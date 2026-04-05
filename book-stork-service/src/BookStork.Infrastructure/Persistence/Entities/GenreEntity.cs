namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class GenreEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public List<BookGenreEntity> BookGenres { get; set; } = [];
}
