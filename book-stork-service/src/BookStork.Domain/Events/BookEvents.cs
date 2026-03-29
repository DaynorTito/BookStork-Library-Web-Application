using BookStork.Domain.Abstractions;

namespace BookStork.Domain.Events;

public sealed record BookCreatedEvent(Guid IdBook, string name, string description, DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}

public sealed record BookUpdatedEvent(
    Guid BookId,
    string Name,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}
 
public sealed record BookDeletedEvent(
    Guid BookId,
    DateTime OccurredOn) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
}
