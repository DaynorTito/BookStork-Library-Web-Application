using AutoMapper;
using BookStork.Application.DTOs;
using FluentValidation;
using MediatR;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;

namespace BookStork.Application.Books.Queries;


public sealed record GetAllBooksQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<ListBookDTO>>;
 
public sealed class GetAllBooksQueryValidator : AbstractValidator<GetAllBooksQuery>
{
    public GetAllBooksQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
 
public sealed class GetAllBooksQueryHandler
    : IRequestHandler<GetAllBooksQuery, PagedResult<ListBookDTO>>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
 
    public GetAllBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }
 
    public async Task<PagedResult<ListBookDTO>> Handle(
        GetAllBooksQuery request,
        CancellationToken cancellationToken)
    {
        var (books, totalCount) = await _bookRepository.GetAllPageAsync(
            request.Page, request.PageSize, cancellationToken);
 
        var dtos = _mapper.Map<List<ListBookDTO>>(books.ToList());
        
        return new PagedResult<ListBookDTO>(dtos, request.Page, request.PageSize, totalCount);
    }
}