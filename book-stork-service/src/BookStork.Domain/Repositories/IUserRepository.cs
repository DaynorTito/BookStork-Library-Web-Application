using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default);
    Task<(IReadOnlyList<User> Users, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    void Update(User user);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
