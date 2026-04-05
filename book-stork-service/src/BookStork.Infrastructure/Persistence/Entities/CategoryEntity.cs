namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<BookEntity> Books { get; set; } = [];
}