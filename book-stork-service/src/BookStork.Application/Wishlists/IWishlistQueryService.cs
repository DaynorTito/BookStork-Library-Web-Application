using BookStork.Application.DTOs;

namespace BookStork.Application.Wishlists;

public interface IWishlistQueryService
{
    Task<IReadOnlyList<WishlistItemDto>> GetByUserAsync(Guid userId, CancellationToken ct = default);
}