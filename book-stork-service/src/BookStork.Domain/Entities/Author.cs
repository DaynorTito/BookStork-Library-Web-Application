using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Author;

namespace BookStork.Domain.Entities;

public sealed class Author : Entity<AuthorId>
{
    private Author() : base(default!) { }
 
    private Author(AuthorId id, string name, string? biography) : base(id)
    {
        Name = name;
        Biography = biography;
    }
 
    public string Name { get; private set; } = string.Empty;
    public string? Biography { get; private set; }
 
    public static Author Create(string name, string? biography = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Author name can not be empty");
        return new Author(AuthorId.New(), name.Trim(), biography?.Trim());
    }
 
    public void Update(string name, string? biography)
    {
        Name = name.Trim();
        Biography = biography?.Trim();
    }
}