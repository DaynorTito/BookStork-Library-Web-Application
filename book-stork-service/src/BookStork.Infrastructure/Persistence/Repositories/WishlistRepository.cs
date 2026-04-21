using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using BookStork.Domain.ValueObjects.Wishlist;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class WishlistRepository : IWishlistRepository
{
    private readonly AppDbContext _ctx;

    public WishlistRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<WishlistItem?> GetAsync(
        UserId userId, BookId bookId, CancellationToken ct = default)
    {
        var e = await _ctx.WishlistItems
            .Include(l => l.Book)
            .FirstOrDefaultAsync(
                w => w.UserId == userId.Value && w.BookId == bookId.Value, ct);

        return e is null ? null : ToDomain(e);
    }

    public async Task<IReadOnlyList<WishlistItem>> GetByUserAsync(
        UserId userId, CancellationToken ct = default)
    {
        var list = await _ctx.WishlistItems
            .Include(l => l.Book)
            .AsNoTracking()
            .Where(w => w.UserId == userId.Value)
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync(ct);

        return list.Select(ToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<WishlistItem>> GetNotifiableByBookAsync(
        BookId bookId, CancellationToken ct = default)
    {
        var list = await _ctx.WishlistItems
            .Include(l => l.Book)
            .AsNoTracking()
            .Where(w => w.BookId == bookId.Value && w.NotifyOnAvailable)
            .ToListAsync(ct);

        return list.Select(ToDomain).ToList().AsReadOnly();
    }

    public Task<bool> ExistsAsync(
        UserId userId, BookId bookId, CancellationToken ct = default)
        => _ctx.WishlistItems.AnyAsync(
            w => w.UserId == userId.Value && w.BookId == bookId.Value, ct);

    public async Task AddAsync(WishlistItem item, CancellationToken ct = default)
        => await _ctx.WishlistItems.AddAsync(ToEntity(item), ct);

    public void Update(WishlistItem item)
    {
        var e = _ctx.WishlistItems.Local
            .FirstOrDefault(x => x.Id == item.Id.Value);

        if (e is null)
        {
            e = ToEntity(item);
            _ctx.WishlistItems.Attach(e);
        }
        else
        {
            e.NotifyOnAvailable = item.NotifyOnAvailable;
        }

        _ctx.Entry(e).State = EntityState.Modified;
    }

    public void Remove(WishlistItem item)
    {
        var e = _ctx.WishlistItems.Local.FirstOrDefault(x => x.Id == item.Id.Value);

            if (e is null)
            {
                e = ToEntity(item);
                _ctx.Entry(e).State = EntityState.Deleted;
            }
            else
            {
                _ctx.WishlistItems.Remove(e);
            }
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);

    private static WishlistItemEntity ToEntity(WishlistItem w) => new()
    {
        Id = w.Id.Value,
        UserId = w.UserId.Value,
        BookId = w.BookId.Value,
        NotifyOnAvailable = w.NotifyOnAvailable,
        AddedAt = w.AddedAt
    };

    private static WishlistItem ToDomain(WishlistItemEntity e)
        => WishlistItem.Reconstitute(
            WishlistItemId.From(e.Id),
            UserId.Create(e.UserId),
            BookId.From(e.BookId),
            e.NotifyOnAvailable,
            e.AddedAt);
}