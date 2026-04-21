using BookStork.Application.Common;
using BookStork.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services;

public sealed class UserSuspendedNotificationHandler : INotificationHandler<DomainEventWrapper<UserSuspendedEvent>>
{
    private readonly ILogger<UserSuspendedNotificationHandler> _logger;
    public UserSuspendedNotificationHandler(ILogger<UserSuspendedNotificationHandler> logger) => _logger = logger;
    public Task Handle(DomainEventWrapper<UserSuspendedEvent> n, CancellationToken ct)
    {
        _logger.LogWarning("[Audit] User suspended: {UserId}", n.DomainEvent.UserId);
        return Task.CompletedTask;
    }
}