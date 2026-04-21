namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class UserBookStatusEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
    public UserEntity User { get; set; } = null!;
    public BookEntity Book { get; set; } = null!;
}
