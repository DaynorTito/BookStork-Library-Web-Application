using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using FluentValidation;
using MediatR;

namespace BookStork.Application.Wishlists;

public sealed record AddToWishlistCommand(
    Guid BookId,
    bool NotifyOnAvailable = true) : IRequest<WishlistItemDtoAction>;

public sealed class AddToWishlistCommandValidator : AbstractValidator<AddToWishlistCommand>
{
    public AddToWishlistCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
    }
}

public sealed class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, WishlistItemDtoAction>
{
    private readonly IWishlistRepository _wishlistRepo;
    private readonly IBookRepository _bookRepo;
    private readonly ICurrentUserService _currentUser;

    public AddToWishlistCommandHandler(
        IWishlistRepository wishlistRepo,
        IBookRepository bookRepo,
        ICurrentUserService currentUser)
    {
        _wishlistRepo = wishlistRepo;
        _bookRepo = bookRepo;
        _currentUser = currentUser;
    }

    public async Task<WishlistItemDtoAction> Handle(AddToWishlistCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(_currentUser.GetCurrentUserId());
        var bookId = BookId.From(request.BookId);

        var book = await _bookRepo.GetByIdAsync(bookId, ct)
                   ?? throw new NotFoundException(nameof(Book), request.BookId);

        if (await _wishlistRepo.ExistsAsync(userId, bookId, ct))
            throw new ConflictException("This book is already in your wishlist.");

        var item = WishlistItem.Create(userId, bookId, request.NotifyOnAvailable);
        await _wishlistRepo.AddAsync(item, ct);
        await _wishlistRepo.SaveChangesAsync(ct);

        return new WishlistItemDtoAction(
            item.Id.Value, item.UserId.Value, item.BookId.Value,
            book.Name, book.Status.Value,
            item.NotifyOnAvailable, item.AddedAt);
    }
}