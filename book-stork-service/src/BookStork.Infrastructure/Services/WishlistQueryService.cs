using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Application.Wishlists;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Services;

public sealed class WishlistQueryService : IWishlistQueryService
{
    private readonly AppDbContext _ctx;
    public WishlistQueryService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IReadOnlyList<WishlistItemDto>> GetByUserAsync(
        Guid userId, CancellationToken ct = default)
    {
        var items = await _ctx.WishlistItems
            .Where(w => w.UserId == userId)
            .Include(w => w.Book)
            .ThenInclude(b => b.Images)
            .Include(w => w.Book)
            .ThenInclude(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(w => w.Book)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync(ct);

        return items.Select(ToDto).ToList().AsReadOnly();
    }

    private static WishlistItemDto ToDto(WishlistItemEntity e) => new()
    {
        Id                = e.Id,
        UserId            = e.UserId,
        Book              = ToBookSummary(e.Book),
        NotifyOnAvailable = e.NotifyOnAvailable,
        AddedAt           = e.AddedAt
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