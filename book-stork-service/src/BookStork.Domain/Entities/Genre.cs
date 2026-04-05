using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Genre;

namespace BookStork.Domain.Entities;

public sealed class Genre : Entity<GenreId>
{
    private Genre() : base(default!) { }
    private Genre(GenreId id, string name) : base(id) { Name = name; }

    public string Name { get; private set; } = string.Empty;

    public static Genre Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Genre name cannot be empty.");
        return new Genre(GenreId.New(), name.Trim());
    }

    public void Update(string name) => Name = name.Trim();
}
