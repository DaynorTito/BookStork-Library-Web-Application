using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;
    public UserRepository(AppDbContext ctx) => _ctx = ctx;
 
    public async Task<User?> GetByIdAsync(UserId id, CancellationToken ct = default)
    {
        var e = await _ctx.Users.AsNoTracking()
            .Include(l => l.Loans)
            .FirstOrDefaultAsync(u => u.Id == id.Value && u.Status != "DELETED", ct);
        return e is null ? null : UserMapper.ToDomain(e, e.Loans.Select(l => l.Id).ToList());
    }
 
    public async Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default)
    {
        var e = await _ctx.Users.AsNoTracking()
            .Include(l => l.Loans)
            .FirstOrDefaultAsync(u => u.Email == email.Value && u.Status != "DELETED", ct);
        return e is null ? null : UserMapper.ToDomain(e, e.Loans.Select(l => l.Id).ToList());
    }
 
    public Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default)
        => _ctx.Users.AnyAsync(u => u.Email == email.Value && u.Status != "DELETED", ct);
 
    public async Task<(IReadOnlyList<User> Users, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Users.AsNoTracking()
            .Include(l => l.Loans)
            .Where(u => u.Status != "DELETED").OrderBy(u => u.LastName);
        var total = await q.CountAsync(ct);
        var list = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (list.Select((us) => UserMapper.ToDomain(us,  us.Loans.Select(l => l.Id).ToList())).ToList().AsReadOnly(), total);
    }
 
    public async Task AddAsync(User user, CancellationToken ct = default)
        => await _ctx.Users.AddAsync(UserMapper.ToEntity(user), ct);
 
    public void Update(User user)
    {
        var e = _ctx.Users.Local.FirstOrDefault(x => x.Id == user.Id.Value)
                ?? _ctx.Users.Attach(UserMapper.ToEntity(user)).Entity;
        UserMapper.Update(e, user);
        _ctx.Entry(e).State = EntityState.Modified;
    }
 
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}