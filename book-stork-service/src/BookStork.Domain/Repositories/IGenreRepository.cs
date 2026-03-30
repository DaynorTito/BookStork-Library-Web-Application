using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Genre;

namespace BookStork.Domain.Repositories;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(GenreId id, CancellationToken ct = default);
    Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Genre genre, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
