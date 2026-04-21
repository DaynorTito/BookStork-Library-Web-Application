using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Author;

namespace BookStork.Domain.Repositories;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(AuthorId id, CancellationToken ct = default);
    Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Author author, CancellationToken ct = default);
    void Update(Author author);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
