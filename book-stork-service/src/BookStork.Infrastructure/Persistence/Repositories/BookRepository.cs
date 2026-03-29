using Microsoft.EntityFrameworkCore;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Infrastructure.Persistence.Mappings;
using UserManagement.Infrastructure.Persistence;

namespace BookStork.Infrastructure.Persistence.Repositories;


public sealed class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
        => _context = context;

    public async Task<Book?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Books
            .Include(b => b.Images)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        return entity is null ? null : BookMapper.ToDomain(entity);
    }

    public async Task<bool> ExistsByISBNAsync(
        string isbn,
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AnyAsync(b => b.ISBN == isbn, cancellationToken);
    }

    public async Task<(IReadOnlyList<Book> Books, int TotalCount)> GetAllPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Books
            .Include(b => b.Images)
            .AsNoTracking()
            .OrderBy(b => b.Name);

        var totalCount = await query.CountAsync(cancellationToken);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var books = entities
            .Select(BookMapper.ToDomain)
            .ToList()
            .AsReadOnly();

        return (books, totalCount);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Books
            .Include(b => b.Images)
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);

        return entities
            .Select(BookMapper.ToDomain)
            .ToList()
            .AsReadOnly();
    }

    public async Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        var entity = BookMapper.ToEntity(book);
        await _context.Books.AddAsync(entity, cancellationToken);
    }

    public void Update(Book book)
    {
        var entity = _context.Books.Local
            .FirstOrDefault(e => e.Id == book.Id);

        if (entity is null)
        {
            entity = BookMapper.ToEntity(book);
            _context.Books.Attach(entity);
        }
        else
        {
            BookMapper.UpdateEntity(entity, book);
        }

        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Books
            .Include(b => b.Images)
            .FirstOrDefaultAsync(b => b.Id == book.Id, cancellationToken);

        if (entity is not null)
            _context.Books.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
