using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Books.Commands;

public sealed record DeleteBookCommand(Guid BookId) : IRequest;
 
public sealed class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty().WithMessage("ID is required");
    }
}

public sealed class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IDomainEventDispatcher _dispatcher;
 
    public DeleteBookCommandHandler(
        IBookRepository bookRepository,
        IDomainEventDispatcher dispatcher)
    {
        _bookRepository = bookRepository;
        _dispatcher = dispatcher;
    }
 
    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
                   ?? throw new NotFoundException(nameof(Book), request.BookId);
        
        await _bookRepository.DeleteAsync(book, cancellationToken);
        await _bookRepository.SaveChangesAsync(cancellationToken);
 
        await _dispatcher.DispatchAsync([book], cancellationToken);
    }
}
