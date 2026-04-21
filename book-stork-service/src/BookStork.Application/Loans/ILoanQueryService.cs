using BookStork.Application.DTOs;

namespace BookStork.Application.Loans;

public interface ILoanQueryService
{
    Task<LoanDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<LoanDto> Items, int TotalCount)> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<(IReadOnlyList<LoanDto> Items, int TotalCount)> GetByUserAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
}
