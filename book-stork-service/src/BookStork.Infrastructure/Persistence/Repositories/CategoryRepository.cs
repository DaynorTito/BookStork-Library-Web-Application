using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _ctx;
    public CategoryRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct = default)
    {
        var e = await _ctx.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id.Value, ct);
        return e is null ? null : CategoryMapper.ToDomain(e);
    }
    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct = default)
        => (await _ctx.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct))
            .Select(CategoryMapper.ToDomain).ToList().AsReadOnly();
    public async Task AddAsync(Category cat, CancellationToken ct = default)
        => await _ctx.Categories.AddAsync(CategoryMapper.ToEntity(cat), ct);
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}