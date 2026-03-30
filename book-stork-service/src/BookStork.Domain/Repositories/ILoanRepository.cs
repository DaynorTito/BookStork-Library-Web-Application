using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Repositories;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(LoanId id, CancellationToken ct = default);
    Task<IReadOnlyList<Loan>> GetActiveByUserAsync(UserId userId, CancellationToken ct = default);
    Task<(IReadOnlyList<Loan> Loans, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<bool> HasActiveLoanAsync(UserId userId, BookId bookId, CancellationToken ct = default);
    Task AddAsync(Loan loan, CancellationToken ct = default);
    void Update(Loan loan);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
