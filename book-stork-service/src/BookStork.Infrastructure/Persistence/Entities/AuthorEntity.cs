namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class AuthorEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Biography { get; set; }
    public List<BookAuthorEntity> BookAuthors { get; set; } = [];
}
