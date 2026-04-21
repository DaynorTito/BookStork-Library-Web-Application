namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class WishlistItemEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public bool NotifyOnAvailable { get; set; }
    public DateTime AddedAt { get; set; }
    public UserEntity User { get; set; } = null!;
    public BookEntity Book { get; set; } = null!;
}