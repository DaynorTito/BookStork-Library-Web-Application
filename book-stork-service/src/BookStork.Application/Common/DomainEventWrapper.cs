using BookStork.Domain.Abstractions;

namespace BookStork.Application.Common;
using MediatR;

public sealed class DomainEventWrapper<TEvent> : INotification
    where TEvent : IDomainEvent
{
    public TEvent DomainEvent { get; }

    public DomainEventWrapper(TEvent domainEvent)
        => DomainEvent = domainEvent;
}
