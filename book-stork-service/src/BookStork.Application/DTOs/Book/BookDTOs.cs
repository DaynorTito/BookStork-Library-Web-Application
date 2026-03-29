namespace BookStork.Application.DTOs.Book;

public sealed class CreateBookDTO
{
    public string ISBN { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; }
    public List<string> Images { get; set; } = new();
}

public sealed class ListBookDTO
{
    public Guid Id { get; set; }
    public string ISBN { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public string Category { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; }
    public List<string> Images { get; set; } = new();
}

public sealed class BookDetailDTO
{
    public Guid Id { get; set; }
    public string ISBN { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; }
    public List<string> Images { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public sealed class UpdateBookDTO
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; }
}

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public PagedResult() {}

    public PagedResult(IReadOnlyList<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
