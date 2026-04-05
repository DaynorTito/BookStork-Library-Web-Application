using AutoMapper;
using MediatR;
using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Reservation;


namespace BookStork.Application.Reservations;

public sealed record CancelReservationCommand(Guid ReservationId) : IRequest<ReservationDto>;
 
public sealed class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _reservationRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;
 
    public CancelReservationCommandHandler(IReservationRepository reservationRepo,
        IBookRepository bookRepo, IDomainEventDispatcher dispatcher, IMapper mapper)
    { _reservationRepo = reservationRepo; _bookRepo = bookRepo; _dispatcher = dispatcher; _mapper = mapper; }
 
    public async Task<ReservationDto> Handle(CancelReservationCommand request, CancellationToken ct)
    {
        var reservation = await _reservationRepo.GetByIdAsync(ReservationId.From(request.ReservationId), ct)
                          ?? throw new NotFoundException(nameof(Reservation), request.ReservationId);
 
        var book = await _bookRepo.GetByIdAsync(reservation.BookId, ct)
                   ?? throw new NotFoundException(nameof(Book), reservation.BookId);
 
        reservation.Cancel();
        book.CancelReservation();
 
        _reservationRepo.Update(reservation);
        _bookRepo.Update(book);
        await _reservationRepo.SaveChangesAsync(ct);
 
        await _dispatcher.DispatchAsync([reservation, book], ct);
 
        return _mapper.Map<ReservationDto>(reservation);
    }
}