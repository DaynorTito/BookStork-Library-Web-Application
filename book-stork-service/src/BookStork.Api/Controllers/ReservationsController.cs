using BookStork.Application.DTOs;
using BookStork.Application.Reservations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/reservations")]
[Produces("application/json")]
[Authorize]
public sealed class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ReservationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllReservationsQuery(page, pageSize), ct));

    [HttpGet("me")]
    [ProducesResponseType(typeof(PagedResult<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetMyReservationsQuery(page, pageSize), ct));

    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest r, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateReservationCommand(r.UserId, r.BookId), ct);
        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new CancelReservationCommand(id), ct));
}