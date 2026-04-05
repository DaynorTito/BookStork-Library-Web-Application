namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class ReservationEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime ReservedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public UserEntity User { get; set; } = null!;
    public BookEntity Book { get; set; } = null!;
}
