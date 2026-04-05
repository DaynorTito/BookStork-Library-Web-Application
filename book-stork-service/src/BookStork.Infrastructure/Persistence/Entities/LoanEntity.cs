namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class LoanEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime LoanedAt { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public UserEntity User { get; set; } = null!;
    public BookEntity Book { get; set; } = null!;
}
