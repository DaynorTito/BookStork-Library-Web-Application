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
 
public sealed record BookDetailDto(
    Guid Id,
    string ISBN,
    string Title,
    Guid AuthorId,
    string AuthorName,
    Guid CategoryId,
    string CategoryName,
    List<GenreDto> Genres,
    string Publisher,
    DateOnly PublishedDate,
    string Description,
    int PageCount,
    decimal Height,
    decimal Weight,
    decimal Thickness,
    string Language,
    decimal AverageRating,
    string Status,
    int AvailableCopies,
    int TotalCopies,
    List<string> Images,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
 
public sealed record CreateBookRequest(
    string ISBN,
    string Title,
    Guid AuthorId,
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
    Guid AuthorId,
    Guid CategoryId,
    List<Guid> GenreIds,
    string Publisher,
    DateOnly PublishedDate,
    string Description,
    int PageCount,
    decimal Height,
    decimal Weight,
    decimal Thickness,
    string Language);
 
public sealed record BookFilterRequest(
    string? Title = null,
    string? AuthorName = null,
    Guid? GenreId = null,
    Guid? CategoryId = null,
    string? Keyword = null,
    string? Language = null,
    string SortBy = "Title",
    bool Ascending = true,
    int Page = 1,
    int PageSize = 10);

