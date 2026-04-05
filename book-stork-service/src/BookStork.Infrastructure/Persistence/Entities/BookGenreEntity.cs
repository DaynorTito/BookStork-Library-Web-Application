namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class BookGenreEntity
{
    public Guid BookId { get; set; }
    public Guid GenreId { get; set; }
    public BookEntity Book { get; set; } = null!;
    public GenreEntity Genre { get; set; } = null!;
}
