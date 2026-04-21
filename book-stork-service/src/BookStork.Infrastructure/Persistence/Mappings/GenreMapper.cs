using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Genre;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class GenreMapper
{
    public static GenreEntity ToEntity(Genre g) => new() { Id = g.Id.Value, Name = g.Name };
    public static Genre ToDomain(GenreEntity e)
    {
        var genre = Genre.Create(e.Name);
        typeof(Genre).GetProperty("Id",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(genre, GenreId.From(e.Id));
        return genre;
    }
}