namespace BookStork.Infrastructure.Persistence.Entities;

public class BookAuthorEntity
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }
    public BookEntity Book { get; set; } = null!;
    public AuthorEntity Author { get; set; } = null!;
}