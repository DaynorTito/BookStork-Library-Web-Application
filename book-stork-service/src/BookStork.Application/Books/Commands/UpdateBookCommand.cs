using BookStork.Application.DTOs.Book;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Domain.ValueObjects.Category;

namespace BookStork.Application.Books.Commands;

using AutoMapper;
using FluentValidation;
using MediatR;

public sealed record UpdateBookCommand(
    Guid BookId, string Title, List<Guid> AuthorIds, Guid CategoryId, List<Guid> GenreIds,
    string Publisher, DateOnly PublishedDate, string Description,
    int PageCount, decimal Height, decimal Weight, decimal Thickness, string Language, decimal AverageRating) : IRequest<BookDetailDto>;
 
public sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Publisher).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PageCount).GreaterThan(0);
        RuleFor(x => x.Height).GreaterThan(0);
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.Thickness).GreaterThan(0);
        RuleFor(x => x.AverageRating).InclusiveBetween(0, 5);
        RuleFor(x => x.Language).NotEmpty().MaximumLength(10);
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}
 
public sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDetailDto>
{
    private readonly IBookRepository _bookRepository;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IMapper _mapper;
 
    public UpdateBookCommandHandler(
        IBookRepository bookRepository,
        IDomainEventDispatcher dispatcher,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _dispatcher = dispatcher;
        _mapper = mapper;
    }
 
    public async Task<BookDetailDto> Handle(
        UpdateBookCommand request,
        CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.BookId);
 
        var authors = request.AuthorIds.Select(AuthorId.From).ToList();
        book.Update(
            request.Title, authors, request.Publisher,
            request.PublishedDate, request.Description, request.PageCount,
            request.Height, request.Weight, request.Thickness,
            CategoryId.From(request.CategoryId), request.Language, request.AverageRating, null);
 
        _bookRepository.Update(book);
 
        await _bookRepository.SaveChangesAsync(cancellationToken);
        await _dispatcher.DispatchAsync([book], cancellationToken);
 
        return _mapper.Map<BookDetailDto>(book);
    }
}