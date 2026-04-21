using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Repositories;

public interface IWishlistRepository
{
    Task<WishlistItem?> GetAsync(UserId userId, BookId bookId, CancellationToken ct = default);
    Task<IReadOnlyList<WishlistItem>> GetByUserAsync(UserId userId, CancellationToken ct = default);

    Task<IReadOnlyList<WishlistItem>> GetNotifiableByBookAsync(BookId bookId, CancellationToken ct = default);

    Task<bool> ExistsAsync(UserId userId, BookId bookId, CancellationToken ct = default);
    Task AddAsync(WishlistItem item, CancellationToken ct = default);
    void Update(WishlistItem item);
    void Remove(WishlistItem item);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}