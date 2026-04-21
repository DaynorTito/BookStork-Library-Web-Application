using Microsoft.EntityFrameworkCore;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Infrastructure.Persistence.Mappings;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;

public sealed class BookRepository : IBookRepository
{
    private readonly AppDbContext _ctx;
    public BookRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<Book?> GetByIdAsync(BookId id, CancellationToken ct = default)
    {
        var e = await _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id.Value, ct);

        return e is null ? null : BookMapper.ToDomain(e);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var e = await _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        return e is null ? null : BookMapper.ToDomain(e);
    }

    public Task<bool> ExistsByISBNAsync(string isbn, CancellationToken ct = default)
        => _ctx.Books.AnyAsync(b => b.ISBN == isbn, ct);

    public async Task<(IReadOnlyList<Book> Books, int TotalCount)> GetAllPageAsync(
        int page, int pageSize, CancellationToken ct = default)
    {
        var query = _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title);

        var total = await query.CountAsync(ct);
        var list = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (list.Select(BookMapper.ToDomain).ToList().AsReadOnly(), total);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync(cancellationToken);

        return list.Select(BookMapper.ToDomain).ToList().AsReadOnly();
    }

    public async Task<(IReadOnlyList<Book> Books, int TotalCount)> GetFilteredAsync(
        BookFilter filter, CancellationToken ct = default)
    {
        var q = _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .Include(b => b.Category)                                
            .AsNoTracking()
            .AsQueryable();

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
        var list = await q
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (list.Select(BookMapper.ToDomain).ToList().AsReadOnly(), total);
    }

    public async Task AddAsync(Book book, CancellationToken ct = default)
        => await _ctx.Books.AddAsync(BookMapper.ToEntity(book), ct);

    public void Update(Book book)
    {
        var e = _ctx.Books.Local.FirstOrDefault(x => x.Id == book.Id.Value);
        if (e is null)
        {
            e = BookMapper.ToEntity(book);
            _ctx.Books.Attach(e);
        }
        else
        {
            BookMapper.Update(e, book);
        }
        _ctx.Entry(e).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Book book, CancellationToken ct = default)
    {
        var e = await _ctx.Books
            .Include(b => b.Images)
            .Include(b => b.BookGenres)
            .Include(b => b.BookAuthors)
            .FirstOrDefaultAsync(b => b.Id == book.Id.Value, ct);
        if (e is not null) _ctx.Books.Remove(e);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
