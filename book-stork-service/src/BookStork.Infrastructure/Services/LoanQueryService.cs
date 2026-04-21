using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Application.Loans;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Services;

public sealed class LoanQueryService : ILoanQueryService
{
    private readonly AppDbContext _ctx;
    public LoanQueryService(AppDbContext ctx) => _ctx = ctx;

    public async Task<LoanDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await BuildBaseQuery().FirstOrDefaultAsync(l => l.Id == id, ct);
        return e is null ? null : ToDto(e);
    }

    public async Task<(IReadOnlyList<LoanDto>, int)> GetAllAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var q = BuildBaseQuery().OrderByDescending(l => l.LoanedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    public async Task<(IReadOnlyList<LoanDto>, int)> GetByUserAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var q = BuildBaseQuery()
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.LoanedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    private IQueryable<LoanEntity> BuildBaseQuery() =>
        _ctx.Loans
            .Include(l => l.Book)
                .ThenInclude(b => b.Images)
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(l => l.Book)
                .ThenInclude(b => b.Category)
            .Include(l => l.User)
            .AsNoTracking();

    private static LoanDto ToDto(LoanEntity e)
    {
        var now = DateTime.UtcNow;
        var daysRemaining = (int)(e.DueDate - now).TotalDays;

        return new LoanDto
        {
            Id           = e.Id,
            UserId       = e.UserId,
            UserFullName = e.User.FirstName + " " + e.User.LastName,
            Book         = ToBookSummary(e.Book),
            Status       = e.Status,
            LoanedAt     = e.LoanedAt,
            DueDate      = e.DueDate,
            ReturnedAt   = e.ReturnedAt,
            IsOverdue    = e.ReturnedAt is null && now > e.DueDate,
            DaysRemaining = daysRemaining < 0 ? 0 : daysRemaining
        };
    }

    private static BookSummaryDto ToBookSummary(BookEntity b) => new()
    {
        Id           = b.Id,
        ISBN         = b.ISBN,
        Title        = b.Title,
        AuthorNames  = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.Name)),
        CategoryName = b.Category.Name,
        Status       = b.Status,
        CoverImageUrl = b.Images.FirstOrDefault(i => i.IsPrimary)?.Url
                        ?? b.Images.FirstOrDefault()?.Url,
        AverageRating = b.AverageRating,
        Language     = b.Language
    };
}