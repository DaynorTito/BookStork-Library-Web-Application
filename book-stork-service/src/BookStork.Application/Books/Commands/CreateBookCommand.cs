using AutoMapper;
using BookStork.Application.DTOs.Book;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Books.Commands;

public sealed record CreateBookCommand(
    string ISBN,
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
    string Language,
    List<string> Images
) : IRequest<Guid>;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Images).NotEmpty();
    }
}

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly IDomainEventDispatcher _eventDispatcher;
    

    public CreateBookCommandHandler(
        IBookRepository bookRepository, 
        IDomainEventDispatcher eventDispatcher,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _eventDispatcher = eventDispatcher;
        _mapper = mapper;
    }
    
    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        if (await _bookRepository.ExistsByISBNAsync(request.ISBN, cancellationToken))
        {
           throw new ConflictException($"Book already exists with ISBN {request.ISBN}");
        }
        
        var bookImages = request.Images.Select(image => new BookImage(image)).ToList();
        
        var book = Book.Create(request.ISBN, request.Name, request.Author, request.Publisher, request.PublishedDate, 
            request.Description, request.PageCount, request.Height, request.Weight, request.Thickness, request.CategoryId,
            request.Language,request.AverageRating, bookImages);
     
        await _bookRepository.AddAsync(book, cancellationToken);
        await _bookRepository.SaveChangesAsync(cancellationToken);
        
       await _eventDispatcher.DispatchAsync([book], cancellationToken);
       
       return book.Id;
    }
}
