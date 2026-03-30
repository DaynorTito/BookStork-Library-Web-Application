using BookStork.Domain.Entities;

namespace BookStork.Domain.Repositories;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByISBNAsync(String ISBN, CancellationToken cancellationToken = default);
    
    Task<(IReadOnlyList<Book> Books, int TotalCount)> GetAllPageAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
 
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<(IReadOnlyList<Book> Books, int TotalCount)> GetFilteredAsync(BookFilter filter, CancellationToken ct = default);
    
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    
    void Update(Book book);
    
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public sealed record BookFilter(
    string? Title = null,
    string? AuthorName = null,
    Guid? GenreId = null,
    Guid? CategoryId = null,
    string? Keyword = null,
    string? Language = null,
    BookSortBy SortBy = BookSortBy.Title,
    bool Ascending = true,
    int Page = 1,
    int PageSize = 10);
 
public enum BookSortBy { Title, PublishedDate, Author, Rating }
