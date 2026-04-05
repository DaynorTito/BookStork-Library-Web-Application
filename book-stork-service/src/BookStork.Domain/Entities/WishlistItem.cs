using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using BookStork.Domain.ValueObjects.Wishlist;

namespace BookStork.Domain.Entities;

public sealed class WishlistItem : Entity<WishlistItemId>
{
    private WishlistItem() : base(default!) { }

    private WishlistItem(WishlistItemId id, UserId userId, BookId bookId, bool notifyOnAvailable, DateTime addedAt)
        : base(id)
    {
        UserId = userId;
        BookId = bookId;
        NotifyOnAvailable = notifyOnAvailable;
        AddedAt = addedAt;
    }

    public UserId UserId { get; private set; } = default!;
    public BookId BookId { get; private set; } = default!;

    public bool NotifyOnAvailable { get; private set; }
    public DateTime AddedAt { get; private set; }

    public static WishlistItem Create(UserId userId, BookId bookId, bool notifyOnAvailable = true)
        => new(WishlistItemId.New(), userId, bookId, notifyOnAvailable, DateTime.UtcNow);

    public void ToggleNotification(bool notify)
        => NotifyOnAvailable = notify;
}
