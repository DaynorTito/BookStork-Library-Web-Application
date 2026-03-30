using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Category;

namespace BookStork.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct = default);
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
