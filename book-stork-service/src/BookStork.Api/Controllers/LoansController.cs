using BookStork.Application.DTOs;
using BookStork.Application.Loans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/loans")]
[Produces("application/json")]
[Authorize]
public sealed class LoansController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoansController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllLoansQuery(page, pageSize), ct));

    [HttpGet("me")]
    [ProducesResponseType(typeof(PagedResult<LoanDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetMyLoansQuery(page, pageSize), ct));

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetLoanByIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateLoanRequest r, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateLoanCommand(r.UserId, r.BookId, r.Days), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(typeof(LoanDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Return([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new ReturnLoanCommand(id), ct));
}