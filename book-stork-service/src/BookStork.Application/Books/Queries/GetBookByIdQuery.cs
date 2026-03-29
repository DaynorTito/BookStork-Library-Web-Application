using BookStork.Domain.Repositories;

namespace BookStork.Application.Books.Queries;

using AutoMapper;
using FluentValidation;
using MediatR;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;

public sealed record GetBookByIdQuery(Guid BookId) : IRequest<BookDetailDTO>;

public sealed class GetBookByIdQueryValidator : AbstractValidator<GetBookByIdQuery>
{
    public GetBookByIdQueryValidator()
    {
        RuleFor(x => x.BookId).NotEmpty().WithMessage("ID Book is required.");
    }
}

public sealed class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailDTO>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<BookDetailDTO> Handle(
        GetBookByIdQuery request,
        CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
                   ?? throw new NotFoundException(nameof(Book), request.BookId);

        return _mapper.Map<BookDetailDTO>(book);
    }
}
