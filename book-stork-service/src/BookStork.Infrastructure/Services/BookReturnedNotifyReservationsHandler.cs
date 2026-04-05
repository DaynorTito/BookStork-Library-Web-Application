using BookStork.Application.Common;
using BookStork.Application.Ports;
using BookStork.Domain.Events;
using BookStork.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services;

public sealed class BookReturnedNotifyReservationsHandler : INotificationHandler<DomainEventWrapper<BookReturnedEvent>>
{
    private readonly IEmailService _email;
    private readonly IReservationRepository _reservationRepo;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<BookReturnedNotifyReservationsHandler> _logger;

    public BookReturnedNotifyReservationsHandler(IEmailService email,
        IReservationRepository reservationRepo, IUserRepository userRepo,
        ILogger<BookReturnedNotifyReservationsHandler> logger)
    { _email = email; _reservationRepo = reservationRepo; _userRepo = userRepo; _logger = logger; }

    public async Task Handle(DomainEventWrapper<BookReturnedEvent> n, CancellationToken ct)
    {
        var evt = n.DomainEvent;

        var pending = await _reservationRepo.GetPendingByBookAsync(evt.BookId, ct);
        if (pending.Count == 0) return;

        var firstReservation = pending[0];
        var user = await _userRepo.GetByIdAsync(firstReservation.UserId, ct);
        if (user is not null)
        {
            _logger.LogInformation("[Reservation] Notifiyng {UserId} that '{Title}' is now available.",
                user.Id, evt.Name);
            await _email.SendReservationAvailableAsync(user.Email.Value, evt.Name, ct);
        }
    }
}
