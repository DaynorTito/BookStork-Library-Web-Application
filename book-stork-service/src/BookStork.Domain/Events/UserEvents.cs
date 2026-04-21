using BookStork.Domain.Abstractions;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Events;

public sealed record UserCreatedEvent(UserId UserId, Email Email, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record UserUpdatedEvent(UserId UserId, Email Email, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record UserSuspendedEvent(UserId UserId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record UserDeletedEvent(UserId UserId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
