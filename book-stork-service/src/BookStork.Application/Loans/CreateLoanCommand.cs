using AutoMapper;
using FluentValidation;
using MediatR;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Application.Loans;

public sealed record CreateLoanCommand(Guid UserId, Guid BookId, int Days = 14) : IRequest<LoanDto>;

public sealed class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Days).InclusiveBetween(1, 60);
    }
}

public sealed class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;

    public CreateLoanCommandHandler(
        ILoanRepository loanRepo, IUserRepository userRepo,
        IBookRepository bookRepo, IReservationRepository reservationRepo,
        IDomainEventDispatcher dispatcher, IMapper mapper)
    {
        _loanRepo = loanRepo; _userRepo = userRepo; _bookRepo = bookRepo;
        _reservationRepo = reservationRepo; _dispatcher = dispatcher; _mapper = mapper;
    }

    public async Task<LoanDto> Handle(CreateLoanCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(request.UserId);
        var bookId = BookId.From(request.BookId);

        var user = await _userRepo.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var book = await _bookRepo.GetByIdAsync(bookId, ct)
            ?? throw new NotFoundException(nameof(Book), request.BookId);

        if (!book.IsAvailable())
            throw new DomainException($"The book '{book.Name}' is not available for loan because its status is '{book.Status.Value}'.");

        if (!user.CanBorrow())
            throw new DomainException($"The user has reached the limit of {user.LoanLimit.Value} simultaneous loans.");

        if (await _loanRepo.HasActiveLoanAsync(userId, bookId, ct))
            throw new ConflictException("The user already has this book on loan.");

        var loan = Loan.Create(userId, bookId, request.Days);
        book.RegisterLoan();
        user.AddActiveLoan(loan.Id);

        var reservation = await _reservationRepo.GetPendingByUserAndBookAsync(userId, bookId, ct);
        if (reservation is not null)
        {
            reservation.Fulfil();
            _reservationRepo.Update(reservation);
        }

        await _loanRepo.AddAsync(loan, ct);
        _userRepo.Update(user);
        _bookRepo.Update(book);
        await _loanRepo.SaveChangesAsync(ct);

        await _dispatcher.DispatchAsync([loan, user, book], ct);

        return _mapper.Map<LoanDto>(loan);
    }
}
