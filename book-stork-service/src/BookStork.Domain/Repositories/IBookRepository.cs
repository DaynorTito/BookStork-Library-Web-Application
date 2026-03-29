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
    
    Task AddAsync(Book book, CancellationToken cancellationToken = default);
    
    void Update(Book book);
    
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
