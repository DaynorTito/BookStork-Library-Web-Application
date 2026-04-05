using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _ctx;
    public AuthorRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken ct = default)
    {
        var e = await _ctx.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id.Value, ct);
        return e is null ? null : AuthorMapper.ToDomain(e);
    }
    public async Task<IReadOnlyList<Author>> GetAllAsync(CancellationToken ct = default)
        => (await _ctx.Authors.AsNoTracking().OrderBy(a => a.Name).ToListAsync(ct))
            .Select(AuthorMapper.ToDomain).ToList().AsReadOnly();
    public async Task AddAsync(Author author, CancellationToken ct = default)
        => await _ctx.Authors.AddAsync(AuthorMapper.ToEntity(author), ct);
    public void Update(Author author)
    {
        var e = AuthorMapper.ToEntity(author);
        _ctx.Authors.Update(e);
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}
