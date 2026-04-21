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


namespace BookStork.Application.Reservations;

public sealed record CreateReservationCommand(Guid UserId, Guid BookId) : IRequest<ReservationDto>;

public sealed class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BookId).NotEmpty();
    }
}

public sealed class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepo;
    private readonly IUserRepository _userRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepo, IUserRepository userRepo,
        IBookRepository bookRepo, IDomainEventDispatcher dispatcher, IMapper mapper)
    {
        _reservationRepo = reservationRepo; _userRepo = userRepo;
        _bookRepo = bookRepo; _dispatcher = dispatcher; _mapper = mapper;
    }

    public async Task<ReservationDto> Handle(CreateReservationCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(request.UserId);
        var bookId = BookId.From(request.BookId);

        var user = await _userRepo.GetByIdAsync(userId, ct)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var book = await _bookRepo.GetByIdAsync(bookId, ct)
            ?? throw new NotFoundException(nameof(Book), request.BookId);

        if (book.IsAvailable())
            throw new DomainException("The book has available copies — you can borrow it directly.");

        var existing = await _reservationRepo.GetPendingByUserAndBookAsync(userId, bookId, ct);
        if (existing is not null)
            throw new ConflictException("You already have a pending reservation for this book.");

        var reservation = Reservation.Create(userId, bookId);
        book.RegisterReservation();

        await _reservationRepo.AddAsync(reservation, ct);
        _bookRepo.Update(book);
        await _reservationRepo.SaveChangesAsync(ct);

        await _dispatcher.DispatchAsync([reservation, book], ct);

        return _mapper.Map<ReservationDto>(reservation);
    }
}
