using BookStork.Application.Ports;
using BookStork.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services;

public sealed class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHubContext<NotificationHub> hubContext,
        ILogger<SignalRNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task NotifyBookAvailableAsync(
        Guid userId,
        Guid bookId,
        string bookTitle,
        CancellationToken ct = default)
    {
        var groupName = $"user_{userId}";

        _logger.LogInformation(
            "[SignalR] Notification book available → user={UserId} book='{Title}'",
            userId, bookTitle);

        await _hubContext.Clients
            .Group(groupName)
            .SendAsync("BookAvailable", new
            {
                BookId = bookId,
                BookTitle = bookTitle,
                Message = $"\"{bookTitle}\" is now available for loan.",
                AvailableAt = DateTime.UtcNow
            }, ct);
    }
}