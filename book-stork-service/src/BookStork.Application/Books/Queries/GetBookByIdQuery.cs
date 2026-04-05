using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;

namespace BookStork.Application.Books.Queries;

using AutoMapper;
using FluentValidation;
using MediatR;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;

public sealed record GetBookByIdQuery(Guid BookId) : IRequest<BookDetailDto>;
 
public sealed class GetBookByIdQueryHandler
    : IRequestHandler<GetBookByIdQuery, BookDetailDto>
{
    private readonly IBookQueryService _query;
    public GetBookByIdQueryHandler(IBookQueryService query) => _query = query;

    public async Task<BookDetailDto> Handle(GetBookByIdQuery r, CancellationToken ct)
        => await _query.GetByIdAsync(r.BookId, ct)
           ?? throw new NotFoundException(nameof(Book), r.BookId);
}
