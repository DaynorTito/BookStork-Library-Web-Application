using BookStork.Application.DTOs.Book;

namespace BookStork.Application.DTOs;

public sealed record WishlistItemDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public BookSummaryDto Book { get; init; } = null!;
    public bool NotifyOnAvailable { get; init; }
    public DateTime AddedAt { get; init; }
}

public sealed record WishlistItemDtoAction(
    Guid Id,
    Guid UserId,
    Guid BookId,
    string BookTitle,
    string BookStatus,
    bool NotifyOnAvailable,
    DateTime AddedAt);

public sealed record AddToWishlistRequest(Guid BookId, bool NotifyOnAvailable = true);
public sealed record ToggleWishlistNotificationRequest(bool NotifyOnAvailable);
