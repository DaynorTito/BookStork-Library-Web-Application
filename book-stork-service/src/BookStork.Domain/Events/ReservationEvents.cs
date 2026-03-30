using BookStork.Domain.Abstractions;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Reservation;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Events;

public sealed record ReservationCreatedEvent(ReservationId ReservationId, UserId UserId, BookId BookId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record ReservationFulfilledEvent(ReservationId ReservationId, UserId UserId, BookId BookId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record ReservationCancelledEvent(ReservationId ReservationId, UserId UserId, BookId BookId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
