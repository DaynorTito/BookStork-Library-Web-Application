using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Repositories;

public interface IUserBookStatusRepository
{
    Task<UserBookStatus?> GetAsync(UserId userId, BookId bookId, CancellationToken ct = default);
    Task<IReadOnlyList<UserBookStatus>> GetByUserAsync(UserId userId, CancellationToken ct = default);
    Task AddAsync(UserBookStatus status, CancellationToken ct = default);
    void Update(UserBookStatus status);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
