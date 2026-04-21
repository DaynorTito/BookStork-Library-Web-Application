using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _ctx;
    public LoanRepository(AppDbContext ctx) => _ctx = ctx;
 
    public async Task<Loan?> GetByIdAsync(LoanId id, CancellationToken ct = default)
    {
        var e = await _ctx.Loans
            .Include(l => l.Book)
            .AsNoTracking().FirstOrDefaultAsync(l => l.Id == id.Value, ct);
        return e is null ? null : LoanMapper.ToDomain(e);
    }
 
    public async Task<IReadOnlyList<Loan>> GetActiveByUserAsync(UserId userId, CancellationToken ct = default)
    {
        var list = await _ctx.Loans
            .Include(l => l.Book)
            .AsNoTracking()
            .Where(l => l.UserId == userId.Value && l.Status == "ACTIVE").ToListAsync(ct);
        return list.Select(LoanMapper.ToDomain).ToList().AsReadOnly();
    }

    public async Task<(IReadOnlyList<Loan> Loans, int TotalCount)> GetAllByUserAsync(UserId userId, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Loans
            .Include(l => l.Book)
            .AsNoTracking().Where(l => l.UserId == userId.Value).OrderByDescending(l => l.LoanedAt);
        var total = await q.CountAsync(ct);
        var list = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (list.Select(LoanMapper.ToDomain).ToList().AsReadOnly(), total);    }

    public async Task<(IReadOnlyList<Loan> Loans, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Loans
            .Include(l => l.Book)
            .AsNoTracking().OrderByDescending(l => l.LoanedAt);
        var total = await q.CountAsync(ct);
        var list = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (list.Select(LoanMapper.ToDomain).ToList().AsReadOnly(), total);
    }
 
    public Task<bool> HasActiveLoanAsync(UserId userId, BookId bookId, CancellationToken ct = default)
        => _ctx.Loans.AnyAsync(l => l.UserId == userId.Value && l.BookId == bookId.Value && l.Status == "ACTIVE", ct);
 
    public async Task AddAsync(Loan loan, CancellationToken ct = default)
        => await _ctx.Loans.AddAsync(LoanMapper.ToEntity(loan), ct);
 
    public void Update(Loan loan)
    {
        var e = _ctx.Loans.Local.FirstOrDefault(x => x.Id == loan.Id.Value);
        if (e is null) { e = LoanMapper.ToEntity(loan); _ctx.Loans.Attach(e); }
        else LoanMapper.Update(e, loan);
        _ctx.Entry(e).State = EntityState.Modified;
    }
 
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
}