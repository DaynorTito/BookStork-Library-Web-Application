using BookStork.Application.Common;
using BookStork.Application.Ports;
using BookStork.Domain.Events;
using MediatR;

namespace BookStork.Infrastructure.Services;

public sealed class UserCreatedNotificationHandler : INotificationHandler<DomainEventWrapper<UserCreatedEvent>>
{
    private readonly IEmailService _email;
    public UserCreatedNotificationHandler(IEmailService email) => _email = email;
    public async Task Handle(DomainEventWrapper<UserCreatedEvent> n, CancellationToken ct)
    {
        var evt = n.DomainEvent;
        await _email.SendWelcomeAsync(evt.Email.Value, "User", ct);
    }

}
