using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Genre;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _ctx;
    public GenreRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<Genre?> GetByIdAsync(GenreId id, CancellationToken ct = default)
    {
        var e = await _ctx.Genres.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id.Value, ct);
        return e is null ? null : GenreMapper.ToDomain(e);
    }
    public async Task<IReadOnlyList<Genre>> GetAllAsync(CancellationToken ct = default)
        => (await _ctx.Genres.AsNoTracking().OrderBy(g => g.Name).ToListAsync(ct))
            .Select(GenreMapper.ToDomain).ToList().AsReadOnly();
    public async Task AddAsync(Genre genre, CancellationToken ct = default)
        => await _ctx.Genres.AddAsync(GenreMapper.ToEntity(genre), ct);
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}