using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Application.Reservations;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Services;

public sealed class ReservationQueryService : IReservationQueryService
{
    private readonly AppDbContext _ctx;
    public ReservationQueryService(AppDbContext ctx) => _ctx = ctx;

    public async Task<ReservationDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await BuildBaseQuery().FirstOrDefaultAsync(r => r.Id == id, ct);
        return e is null ? null : ToDto(e);
    }

    public async Task<(IReadOnlyList<ReservationDto>, int)> GetAllAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var q = BuildBaseQuery().OrderByDescending(r => r.ReservedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    public async Task<(IReadOnlyList<ReservationDto>, int)> GetByUserAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var q = BuildBaseQuery()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.ReservedAt);
        var total = await q.CountAsync(ct);
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    private IQueryable<ReservationEntity> BuildBaseQuery() =>
        _ctx.Reservations
            .Include(r => r.Book)
                .ThenInclude(b => b.Images)
            .Include(r => r.Book)
                .ThenInclude(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(r => r.Book)
                .ThenInclude(b => b.Category)
            .Include(r => r.User)
            .AsNoTracking();

    private static ReservationDto ToDto(ReservationEntity e) => new()
    {
        Id           = e.Id,
        UserId       = e.UserId,
        UserFullName = e.User.FirstName + " " + e.User.LastName,
        Book         = ToBookSummary(e.Book),
        Status       = e.Status,
        ReservedAt   = e.ReservedAt,
        ExpiresAt    = e.ExpiresAt,
        FulfilledAt  = e.FulfilledAt
    };

    private static BookSummaryDto ToBookSummary(BookEntity b) => new()
    {
        Id            = b.Id,
        ISBN          = b.ISBN,
        Title         = b.Title,
        AuthorNames   = string.Join(", ", b.BookAuthors.Select(ba => ba.Author.Name)),
        CategoryName  = b.Category.Name,
        Status        = b.Status,
        CoverImageUrl = b.Images.FirstOrDefault(i => i.IsPrimary)?.Url
                        ?? b.Images.FirstOrDefault()?.Url,
        AverageRating = b.AverageRating,
        Language      = b.Language
    };
}