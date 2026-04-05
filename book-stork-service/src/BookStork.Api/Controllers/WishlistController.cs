using BookStork.Application.DTOs;
using BookStork.Application.Wishlists;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/wishlist")]
[Produces("application/json")]
[Authorize]
public sealed class WishlistController : ControllerBase
{
    private readonly IMediator _mediator;
    public WishlistController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<WishlistItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken ct)
        => Ok(await _mediator.Send(new GetMyWishlistQuery(), ct));


    [HttpPost]
    [ProducesResponseType(typeof(WishlistItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add(
        [FromBody] AddToWishlistRequest r, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new AddToWishlistCommand(r.BookId, r.NotifyOnAvailable), ct);
        return CreatedAtAction(nameof(GetMine), result);
    }

    [HttpDelete("{bookId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] Guid bookId, CancellationToken ct)
    {
        await _mediator.Send(new RemoveFromWishlistCommand(bookId), ct);
        return NoContent();
    }


    [HttpPatch("{bookId:guid}/notify")]
    [ProducesResponseType(typeof(WishlistItemDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ToggleNotify(
        [FromRoute] Guid bookId,
        [FromBody] ToggleWishlistNotificationRequest r,
        CancellationToken ct)
        => Ok(await _mediator.Send(
            new ToggleWishlistNotificationCommand(bookId, r.NotifyOnAvailable), ct));
}
