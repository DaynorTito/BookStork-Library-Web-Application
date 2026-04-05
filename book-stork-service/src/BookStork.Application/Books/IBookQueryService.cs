using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;

namespace BookStork.Application.Books;

public interface IBookQueryService
{
    Task<(IReadOnlyList<BookDetailDto> Items, int TotalCount)> GetAllPagedAsync(
        int page, int pageSize, CancellationToken ct = default);

    Task<(IReadOnlyList<BookDetailDto> Items, int TotalCount)> GetFilteredAsync(
        BookFilter filter, CancellationToken ct = default);

    Task<BookDetailDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
