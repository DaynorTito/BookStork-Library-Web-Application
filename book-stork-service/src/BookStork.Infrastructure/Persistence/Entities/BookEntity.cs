namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class BookEntity
{
    public Guid Id { get; set; }
    public string ISBN { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Publisher { get; set; } = null!;
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; } = null!;
    public int PageCount { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public Guid CategoryId { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<BookImageEntity> Images { get; set; } = new();
}
