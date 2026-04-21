using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class UserBookStatusRepository : IUserBookStatusRepository
{
    private readonly AppDbContext _ctx;
    public UserBookStatusRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<UserBookStatus?> GetAsync(UserId userId, BookId bookId, CancellationToken ct = default)
    {
        var e = await _ctx.UserBookStatuses.AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId.Value && s.BookId == bookId.Value, ct);
        if (e is null) return null;
        return UserBookStatus.Create(userId, bookId, e.Status);
    }
    public async Task<IReadOnlyList<UserBookStatus>> GetByUserAsync(UserId userId, CancellationToken ct = default)
    {
        var list = await _ctx.UserBookStatuses.AsNoTracking()
            .Where(s => s.UserId == userId.Value).ToListAsync(ct);
        return list.Select(e => UserBookStatus.Create(userId, BookId.From(e.BookId), e.Status))
            .ToList().AsReadOnly();
    }
    public async Task AddAsync(UserBookStatus status, CancellationToken ct = default)
        => await _ctx.UserBookStatuses.AddAsync(new UserBookStatusEntity
        {
            Id = Guid.NewGuid(), UserId = status.UserId.Value, BookId = status.BookId.Value,
            Status = status.Status.Value, UpdatedAt = status.UpdatedAt
        }, ct);
    public void Update(UserBookStatus status)
    {
        var e = _ctx.UserBookStatuses.Local.FirstOrDefault(
            x => x.UserId == status.UserId.Value && x.BookId == status.BookId.Value);
        if (e is not null) { e.Status = status.Status.Value; e.UpdatedAt = status.UpdatedAt; }
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}