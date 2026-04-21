using AutoMapper;
using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Application.Books.Queries;
public sealed record GetFilteredBooksQuery(
    string? Title = null,
    Guid? AuthorId = null,
    Guid? GenreId = null,
    Guid? CategoryId = null,
    string? Keyword = null,
    string? Language = null,
    string SortBy = "Title",
    bool Ascending = true,
    int Page = 1,
    int PageSize = 10) : IRequest<PagedResult<BookDetailDto>>;
 
public sealed class GetFilteredBooksQueryHandler
    : IRequestHandler<GetFilteredBooksQuery, PagedResult<BookDetailDto>>
{
    private readonly IBookQueryService _query;
    public GetFilteredBooksQueryHandler(IBookQueryService query) => _query = query;

    public async Task<PagedResult<BookDetailDto>> Handle(
        GetFilteredBooksQuery r, CancellationToken ct)
    {
        var sortBy = r.SortBy.ToUpper() switch
        {
            "PUBLISHEDDATE" => BookSortBy.PublishedDate,
            "AUTHOR"        => BookSortBy.Author,
            "RATING"        => BookSortBy.Rating,
            _               => BookSortBy.Title
        };

        var filter = new BookFilter(
            r.Title, r.AuthorId, r.GenreId, r.CategoryId,
            r.Keyword, r.Language, sortBy, r.Ascending, r.Page, r.PageSize);

        var (items, total) = await _query.GetFilteredAsync(filter, ct);
        return new PagedResult<BookDetailDto>(items, r.Page, r.PageSize, total);
    }
}