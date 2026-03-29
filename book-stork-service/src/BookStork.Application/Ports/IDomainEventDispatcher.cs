using BookStork.Domain.Abstractions;
using BookStork.Domain.Entities;

namespace BookStork.Application.Ports;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken = default);
}
