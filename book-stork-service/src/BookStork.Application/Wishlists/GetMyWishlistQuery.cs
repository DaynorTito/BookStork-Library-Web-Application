using BookStork.Application.DTOs;
using BookStork.Application.Ports;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.User;
using MediatR;

namespace BookStork.Application.Wishlists;

public sealed record GetMyWishlistQuery : IRequest<IReadOnlyList<WishlistItemDto>>;
 
public sealed class GetMyWishlistQueryHandler : IRequestHandler<GetMyWishlistQuery, IReadOnlyList<WishlistItemDto>>
{
    private readonly IWishlistQueryService _query;
    private readonly ICurrentUserService _currentUser;
    public GetMyWishlistQueryHandler(IWishlistQueryService query, ICurrentUserService currentUser)
        => (_query, _currentUser) = (query, currentUser);

    public async Task<IReadOnlyList<WishlistItemDto>> Handle(GetMyWishlistQuery r, CancellationToken ct)
        => await _query.GetByUserAsync(_currentUser.GetCurrentUserId(), ct);
}