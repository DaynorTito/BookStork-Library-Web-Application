using BookStork.Application.Common;
using BookStork.Application.Ports;
using BookStork.Domain.Abstractions;

namespace BookStork.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;
public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;
 
    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
 
    public async Task DispatchAsync(
        IEnumerable<AggregateRoot> aggregates,
        CancellationToken cancellationToken = default)
    {
        foreach (var aggregate in aggregates)
        {
            var events = aggregate.DomainEvents.ToList();
            aggregate.ClearDomainEvents();
 
            foreach (var domainEvent in events)
            {
                _logger.LogInformation(
                    "[DomainEvent] Published {EventType} [{EventId}] for agregate",
                    domainEvent.GetType().Name, domainEvent.EventId);
                
                var wrapperType = typeof(DomainEventWrapper<>).MakeGenericType(domainEvent.GetType());
                var wrapper = (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;
                
                await _mediator.Publish(wrapper, cancellationToken);
            }
        }
    }
}
