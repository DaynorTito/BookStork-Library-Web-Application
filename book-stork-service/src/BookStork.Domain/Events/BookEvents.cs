using BookStork.Domain.Abstractions;
using BookStork.Domain.ValueObjects.Book;

namespace BookStork.Domain.Events;

public sealed record BookCreatedEvent(BookId IdBook, string name, string description, DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}

public sealed record BookUpdatedEvent(
    BookId BookId,
    string Name,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}

public sealed record BookDeletedEvent(
    BookId BookId,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}

public sealed record BookReturnedEvent(BookId BookId, string Name, int AvailableCopies, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
