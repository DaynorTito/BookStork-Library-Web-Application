using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Loan;
using MediatR;

namespace BookStork.Application.Loans;

public sealed record ReturnLoanCommand(Guid LoanId) : IRequest<LoanDto>;

public sealed class ReturnLoanCommandHandler : IRequestHandler<ReturnLoanCommand, LoanDto>
{
    private readonly ILoanRepository _loanRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IReservationRepository _reservationRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;

    public ReturnLoanCommandHandler(
        ILoanRepository loanRepo, IUserRepository userRepo,
        IBookRepository bookRepo, IReservationRepository reservationRepo,
        IDomainEventDispatcher dispatcher, IMapper mapper)
    {
        _loanRepo = loanRepo; _userRepo = userRepo; _bookRepo = bookRepo;
        _reservationRepo = reservationRepo; _dispatcher = dispatcher; _mapper = mapper;
    }

    public async Task<LoanDto> Handle(ReturnLoanCommand request, CancellationToken ct)
    {
        var loanId = LoanId.Create(request.LoanId);
        var loan = await _loanRepo.GetByIdAsync(loanId, ct)
            ?? throw new NotFoundException(nameof(Loan), request.LoanId);

        var user = await _userRepo.GetByIdAsync(loan.UserId, ct)
            ?? throw new NotFoundException(nameof(User), loan.UserId.Value);

        var book = await _bookRepo.GetByIdAsync(loan.BookId, ct)
            ?? throw new NotFoundException(nameof(Book), loan.BookId);

        loan.Return();
        book.RegisterReturn();
        user.RemoveActiveLoan(loan.Id);

        var pendingReservations = await _reservationRepo.GetPendingByBookAsync(book.Id, ct);
        if (pendingReservations.Count > 0)
            book.RegisterReservation();

        _loanRepo.Update(loan);
        _userRepo.Update(user);
        _bookRepo.Update(book);
        await _loanRepo.SaveChangesAsync(ct);

        await _dispatcher.DispatchAsync([loan, user, book], ct);

        return _mapper.Map<LoanDto>(loan);
    }
}
