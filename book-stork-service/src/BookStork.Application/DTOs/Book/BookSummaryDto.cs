namespace BookStork.Application.DTOs.Book;


public sealed record BookSummaryDto
{
    public Guid Id { get; init; }
    public string ISBN { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string AuthorNames { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public decimal AverageRating { get; init; }
    public string Language { get; init; } = string.Empty;
}