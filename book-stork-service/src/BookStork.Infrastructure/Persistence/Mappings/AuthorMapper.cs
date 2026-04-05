using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class AuthorMapper
{
    public static AuthorEntity ToEntity(Author a) => new() { Id = a.Id.Value, Name = a.Name, Biography = a.Biography };
    public static Author ToDomain(AuthorEntity e)
    {
        var author = Author.Create(e.Name, e.Biography);
        typeof(Author).GetProperty("Id",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(author, AuthorId.From(e.Id));
        return author;
    }
}