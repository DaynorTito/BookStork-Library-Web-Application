using BookStork.Application.Books;
using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;
using BookStork.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Queries;

public sealed class BookQueryService : IBookQueryService
{
    private readonly AppDbContext _ctx;

    public BookQueryService(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IReadOnlyList<BookDetailDto>, int)> GetAllPagedAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var query = BuildBaseQuery().OrderBy(b => b.Title);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    public async Task<(IReadOnlyList<BookDetailDto>, int)> GetFilteredAsync(
        BookFilter filter, CancellationToken ct = default)
    {
        var q = BuildBaseQuery();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            q = q.Where(b => b.Title.Contains(filter.Title));

        if (filter.AuthorId.HasValue)
            q = q.Where(b => b.BookAuthors.Any(ba => ba.AuthorId == filter.AuthorId.Value));

        if (filter.GenreId.HasValue)
            q = q.Where(b => b.BookGenres.Any(bg => bg.GenreId == filter.GenreId.Value));

        if (filter.CategoryId.HasValue)
            q = q.Where(b => b.CategoryId == filter.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(filter.Language))
            q = q.Where(b => b.Language == filter.Language.ToLower());

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
            q = q.Where(b =>
                b.Title.Contains(filter.Keyword) ||
                b.Description.Contains(filter.Keyword) ||
                b.BookAuthors.Any(ba => ba.Author.Name.Contains(filter.Keyword)));

        q = filter.SortBy switch
        {
            BookSortBy.PublishedDate => filter.Ascending
                ? q.OrderBy(b => b.PublishedDate)
                : q.OrderByDescending(b => b.PublishedDate),
            BookSortBy.Author => filter.Ascending
                ? q.OrderBy(b => b.BookAuthors.FirstOrDefault()!.Author.Name)
                : q.OrderByDescending(b => b.BookAuthors.FirstOrDefault()!.Author.Name),
            BookSortBy.Rating => filter.Ascending
                ? q.OrderBy(b => b.AverageRating)
                : q.OrderByDescending(b => b.AverageRating),
            _ => filter.Ascending
                ? q.OrderBy(b => b.Title)
                : q.OrderByDescending(b => b.Title)
        };

        var total = await q.CountAsync(ct);
        var items = await q
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (items.Select(ToDto).ToList().AsReadOnly(), total);
    }

    public async Task<BookDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await BuildBaseQuery()
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        return e is null ? null : ToDto(e);
    }


    private IQueryable<BookEntity> BuildBaseQuery() =>
        _ctx.Books
            .Include(b => b.Category)
            .Include(b => b.Images)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .AsNoTracking();

    private static BookDetailDto ToDto(BookEntity e) => new()
    {
        Id            = e.Id,
        ISBN          = e.ISBN,
        Title         = e.Title,
        Authors       = e.BookAuthors.Select(ba => new AuthorDto
                        {
                            Id        = ba.Author.Id,
                            Name      = ba.Author.Name,
                            Biography = ba.Author.Biography
                        }).ToList(),
        Category      = new CategoryDto
                        {
                            Id          = e.Category.Id,
                            Name        = e.Category.Name,
                            Description = e.Category.Description
                        },
        Genres        = e.BookGenres.Select(bg => new GenreDto
                        {
                            Id   = bg.Genre.Id,
                            Name = bg.Genre.Name
                        }).ToList(),
        Publisher     = e.Publisher,
        PublishedDate = e.PublishedDate,
        Description   = e.Description,
        PageCount     = e.PageCount,
        Height        = e.Height,
        Weight        = e.Weight,
        Thickness     = e.Thickness,
        Language      = e.Language,
        AverageRating = e.AverageRating,
        Status        = e.Status,
        AvailableCopies = e.AvailableCopies,
        TotalCopies   = e.TotalCopies,
        Images        = e.Images.OrderByDescending(i => i.IsPrimary)
                               .Select(i => i.Url).ToList(),
        CreatedAt     = e.CreatedAt,
        UpdatedAt     = e.UpdatedAt
    };
}