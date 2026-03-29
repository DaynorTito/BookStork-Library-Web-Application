namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class BookImageEntity
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public BookEntity Book { get; set; } = null!;
}
