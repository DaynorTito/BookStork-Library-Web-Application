using BookStork.Domain.ValueObjects.Author;

namespace BookStork.Application.DTOs.Book;

public sealed record BookListDto(
    Guid Id,
    string ISBN,
    string Title,
    string AuthorName,
    string CategoryName,
    List<string> Genres,
    string Language,
    decimal AverageRating,
    string Status,
    int AvailableCopies,
    int TotalCopies,
    DateOnly PublishedDate,
    string? CoverImageUrl);
 
public sealed record BookDetailDto
{
    public Guid Id { get; init; }
    public string ISBN { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public List<AuthorDto> Authors { get; init; } = [];
    public CategoryDto Category { get; init; } = new();
    public List<GenreDto> Genres { get; init; } = [];
    public string Publisher { get; init; } = string.Empty;
    public DateOnly PublishedDate { get; init; }
    public string Description { get; init; } = string.Empty;
    public int PageCount { get; init; }
    public decimal Height { get; init; }
    public decimal Weight { get; init; }
    public decimal Thickness { get; init; }
    public string Language { get; init; } = string.Empty;
    public decimal AverageRating { get; init; }
    public string Status { get; init; } = string.Empty;
    public int AvailableCopies { get; init; }
    public int TotalCopies { get; init; }
    public List<string> Images { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
 
public sealed record CreateBookRequest(
    string ISBN,
    string Title,
    List<Guid> AuthorIds,
    Guid CategoryId,
    List<Guid> GenreIds,
    string Publisher,
    DateOnly PublishedDate,
    string Description,
    int PageCount,
    decimal Height,
    decimal Weight,
    decimal Thickness,
    string Language,
    decimal AverageRating,
    int TotalCopies,
    List<string> Images);
 
public sealed record UpdateBookRequest(
    string Title,
    List<Guid> AuthorIds,
    Guid CategoryId,
    List<Guid> GenreIds,
    string Publisher,
    DateOnly PublishedDate,
    string Description,
    int PageCount,
    decimal AverageRating,
    decimal Height,
    decimal Weight,
    decimal Thickness,
    string Language);
 
public sealed record BookFilterRequest(
    string? Title = null,
    Guid? AuthorId = null,
    Guid? GenreId = null,
    Guid? CategoryId = null,
    string? Keyword = null,
    string? Language = null,
    string SortBy = "Title",
    bool Ascending = true,
    int Page = 1,
    int PageSize = 10);

