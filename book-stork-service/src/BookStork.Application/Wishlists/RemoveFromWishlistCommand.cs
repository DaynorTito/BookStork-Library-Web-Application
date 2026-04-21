using BookStork.Application.Ports;
using BookStork.Domain.Exceptions;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using MediatR;

namespace BookStork.Application.Wishlists;

public sealed record RemoveFromWishlistCommand(Guid BookId) : IRequest;

public sealed class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand>
{
    private readonly IWishlistRepository _wishlistRepo;
    private readonly ICurrentUserService _currentUser;

    public RemoveFromWishlistCommandHandler(
        IWishlistRepository wishlistRepo,
        ICurrentUserService currentUser)
    {
        _wishlistRepo = wishlistRepo;
        _currentUser = currentUser;
    }

    public async Task Handle(RemoveFromWishlistCommand request, CancellationToken ct)
    {
        var userId = UserId.Create(_currentUser.GetCurrentUserId());
        var bookId = BookId.From(request.BookId);

        var item = await _wishlistRepo.GetAsync(userId, bookId, ct)
                   ?? throw new NotFoundException("WishlistItem", request.BookId);
        _wishlistRepo.Remove(item);
        await _wishlistRepo.SaveChangesAsync(ct);
    }
}