using AutoMapper;
using BookStork.Application.DTOs.Book;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Domain.ValueObjects.Genre;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStork.Application.Books.Commands;

public sealed record CreateBookCommand(
    string ISBN, string Title, List<Guid> AuthorId, Guid CategoryId, List<Guid> GenreIds,
    string Publisher, DateOnly PublishedDate, string Description, int PageCount,
    decimal Height, decimal Weight, decimal Thickness, string Language,
    decimal AverageRating, int TotalCopies, List<string> Images) : IRequest<Guid>;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AuthorId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Publisher).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.PageCount).GreaterThan(0);
        RuleFor(x => x.TotalCopies).GreaterThan(0);
        RuleFor(x => x.AverageRating).InclusiveBetween(0, 5);
        RuleFor(x => x.Images).NotEmpty().WithMessage("At least one image is required.");
    }
}

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _bookRepo;
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly ILogger _logger;

    public CreateBookCommandHandler(IBookRepository bookRepo, IDomainEventDispatcher dispatcher, ILogger<CreateBookCommandHandler> logger)
    { _bookRepo = bookRepo; _dispatcher = dispatcher; _logger = logger; }

    public async Task<Guid> Handle(CreateBookCommand r, CancellationToken ct)
    {
        _logger.LogInformation("Creating a new book");
        if (await _bookRepo.ExistsByISBNAsync(r.ISBN, ct))
            throw new ConflictException($"A book with ISBN '{r.ISBN}' already exists.");
        _logger.LogInformation("Pass validation");

        var genreIds = r.GenreIds.Select(GenreId.From).ToList();
        var images = r.Images.Select(img => new BookImage(img)).ToList();
        _logger.LogInformation("Pass MAPS");

        var book = Book.Create(r.ISBN, r.Title, r.AuthorId.Select(id => AuthorId.From(id)).ToList(), r.Publisher,
            r.PublishedDate, r.Description, r.PageCount, r.Height, r.Weight, r.Thickness, CategoryId.From(r.CategoryId), r.Language,
            r.TotalCopies, r.AverageRating, images, genreIds);
        _logger.LogInformation("Pass CREATE");

        await _bookRepo.AddAsync(book, ct);
        await _bookRepo.SaveChangesAsync(ct);
        await _dispatcher.DispatchAsync([book], ct);

        return book.Id.Value;
    }
}
