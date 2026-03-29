using BookStork.Application.DTOs.Book;
using BookStork.Domain.Repositories;

namespace BookStork.Application.Books.Commands;

using AutoMapper;
using FluentValidation;
using MediatR;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
 

public sealed record UpdateBookCommand(
    Guid BookId,
    string Name,
    string Author,
    string Publisher,
    DateOnly PublishedDate,
    string Description,
    int PageCount,
    Guid CategoryId,
    decimal Height,
    decimal Weight,
    decimal Thickness,
    decimal AverageRating,
    string Language
) : IRequest<BookDetailDTO>;
 
public sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(254);
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
 
public sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDetailDTO>
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
 
    public async Task<BookDetailDTO> Handle(
        UpdateBookCommand request,
        CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.BookId);
 
        book.Update(
            request.Name, request.Author, request.Publisher,
            request.PublishedDate, request.Description, request.PageCount,
            request.Height, request.Weight, request.Thickness,
            request.CategoryId, request.Language, request.AverageRating);
 
        _bookRepository.Update(book);
 
        await _bookRepository.SaveChangesAsync(cancellationToken);
        await _dispatcher.DispatchAsync([book], cancellationToken);
 
        return _mapper.Map<BookDetailDTO>(book);
    }
}