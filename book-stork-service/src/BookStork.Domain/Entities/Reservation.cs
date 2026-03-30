using BookStork.Domain.Events;
using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Reservation;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Entities;

public sealed class Reservation : Entity<ReservationId>
{
    private Reservation() : base(default!) { }
 
    private Reservation(ReservationId id, UserId userId, BookId bookId, DateTime reservedAt, DateTime expiresAt) : base(id)
    {
        UserId = userId;
        BookId = bookId;
        ReservedAt = reservedAt;
        ExpiresAt = expiresAt;
        Status = ReservationStatus.Pending;
    }
 
    public UserId UserId { get; private set; } = default!;
    public BookId BookId { get; private set; } = default!;
    public ReservationStatus Status { get; private set; } = default!;
    public DateTime ReservedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? FulfilledAt { get; private set; }
 
    public static Reservation Create(UserId userId, BookId bookId, int expirationDays = 3)
    {
        var id = ReservationId.New();
        var now = DateTime.UtcNow;
        var reservation = new Reservation(id, userId, bookId, now, now.AddDays(expirationDays));
        reservation.RaiseDomainEvent(new ReservationCreatedEvent(id, userId, bookId, now));
        return reservation;
    }
 
    public void Fulfil()
    {
        if (Status != ReservationStatus.Pending)
            throw new DomainException("Solo reservas pendientes pueden cumplirse.");
        Status = ReservationStatus.Fulfilled;
        FulfilledAt = DateTime.UtcNow;
        RaiseDomainEvent(new ReservationFulfilledEvent(Id, UserId, BookId, FulfilledAt.Value));
    }
 
    public void Cancel()
    {
        if (Status == ReservationStatus.Cancelled)
            throw new DomainException("La reserva ya fue cancelada.");
        if (Status == ReservationStatus.Fulfilled)
            throw new DomainException("No se puede cancelar una reserva cumplida.");
        Status = ReservationStatus.Cancelled;
        RaiseDomainEvent(new ReservationCancelledEvent(Id, UserId, BookId, DateTime.UtcNow));
    }
 
    public bool IsExpired() => DateTime.UtcNow > ExpiresAt && Status == ReservationStatus.Pending;
}
