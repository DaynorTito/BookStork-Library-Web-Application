using BookStork.Application.Common;
using BookStork.Application.Ports;
using BookStork.Domain.Events;
using BookStork.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services;


public sealed class BookReturnedWishlistNotifier
    : INotificationHandler<DomainEventWrapper<BookReturnedEvent>>
{
    private readonly IWishlistRepository _wishlistRepo;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepo;
    private readonly ILogger<BookReturnedWishlistNotifier> _logger;

    public BookReturnedWishlistNotifier(
        IWishlistRepository wishlistRepo,
        INotificationService notificationService,
        IEmailService emailService,
        IUserRepository userRepo,
        ILogger<BookReturnedWishlistNotifier> logger)
    {
        _wishlistRepo = wishlistRepo;
        _notificationService = notificationService;
        _emailService = emailService;
        _userRepo = userRepo;
        _logger = logger;
    }

    public async Task Handle(
        DomainEventWrapper<BookReturnedEvent> notification,
        CancellationToken ct)
    {
        var evt = notification.DomainEvent;

        if (evt.AvailableCopies <= 0) return;

        var wishlistItems = await _wishlistRepo.GetNotifiableByBookAsync(evt.BookId, ct);

        if (wishlistItems.Count == 0) return;

        _logger.LogInformation(
            "[Wishlist] Books '{Title}' available — notifying {Count} user(s)",
            evt.Name, wishlistItems.Count);

        foreach (var item in wishlistItems)
        {
            await _notificationService.NotifyBookAvailableAsync(
                item.UserId.Value, evt.BookId.Value, evt.Name, ct);

            var user = await _userRepo.GetByIdAsync(item.UserId, ct);
            if (user is not null &&
                user.NotificationPreference.Value == "EMAIL")
            {
                await _emailService.SendReservationAvailableAsync(
                    user.Email.Value, evt.Name, ct);
            }
        }
    }
}
