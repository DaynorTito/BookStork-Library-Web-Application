using AutoMapper;
using BookStork.Application.DTOs;
using FluentValidation;
using MediatR;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;

namespace BookStork.Application.Books.Queries;


public sealed record GetAllBooksQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<BookDetailDto>>;
 
public sealed class GetAllBooksQueryValidator : AbstractValidator<GetAllBooksQuery>
{
    public GetAllBooksQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
 
public sealed class GetAllBooksQueryHandler
    : IRequestHandler<GetAllBooksQuery, PagedResult<BookDetailDto>>
{
    private readonly IBookQueryService _query;
    public GetAllBooksQueryHandler(IBookQueryService query) => _query = query;

    public async Task<PagedResult<BookDetailDto>> Handle(
        GetAllBooksQuery r, CancellationToken ct)
    {
        var (items, total) = await _query.GetAllPagedAsync(r.Page, r.PageSize, ct);
        return new PagedResult<BookDetailDto>(items, r.Page, r.PageSize, total);
    }
}