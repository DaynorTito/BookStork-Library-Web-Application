using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Entities;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using MediatR;

namespace BookStork.Application.Wishlists;

public sealed record ToggleWishlistNotificationCommand(
    Guid BookId,
    bool NotifyOnAvailable) : IRequest<WishlistItemDtoAction>;
 
public sealed class ToggleWishlistNotificationCommandHandler
    : IRequestHandler<ToggleWishlistNotificationCommand, WishlistItemDtoAction>
{
    private readonly IWishlistRepository _wishlistRepo;
    private readonly IBookRepository _bookRepo;
    private readonly ICurrentUserService _currentUser;
 
    public ToggleWishlistNotificationCommandHandler(
        IWishlistRepository wishlistRepo,
        IBookRepository bookRepo,
        ICurrentUserService currentUser)
    {
        _wishlistRepo = wishlistRepo;
        _bookRepo = bookRepo;
        _currentUser = currentUser;
    }
 
    public async Task<WishlistItemDtoAction> Handle(
        ToggleWishlistNotificationCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(_currentUser.GetCurrentUserId());
        var bookId = BookId.From(request.BookId);
 
        var item = await _wishlistRepo.GetAsync(userId, bookId, ct)
                   ?? throw new NotFoundException("WishlistItem", request.BookId);
 
        var book = await _bookRepo.GetByIdAsync(bookId, ct)
                   ?? throw new NotFoundException(nameof(Book), request.BookId);
 
        item.ToggleNotification(request.NotifyOnAvailable);
        _wishlistRepo.Update(item);
        await _wishlistRepo.SaveChangesAsync(ct);
 
        return new WishlistItemDtoAction(
            item.Id.Value, item.UserId.Value, item.BookId.Value,
            book.Name, book.Status.Value,
            item.NotifyOnAvailable, item.AddedAt);
    }
}
