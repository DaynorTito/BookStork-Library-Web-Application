namespace BookStork.Application.Ports;

public interface INotificationService
{
    Task NotifyBookAvailableAsync(
        Guid userId,
        Guid bookId,
        string bookTitle,
        CancellationToken ct = default);
}
