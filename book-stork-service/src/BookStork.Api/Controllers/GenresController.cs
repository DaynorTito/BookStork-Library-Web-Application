using BookStork.Application.BookDetails;
using BookStork.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/genres")]
[Produces("application/json")]
public sealed class GenresController : ControllerBase
{
    private readonly IMediator _mediator;
    public GenresController(IMediator mediator) => _mediator = mediator;
 
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllGenresQuery(), ct));
 
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateGenreRequest r, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateGenreCommand(r.Name), ct));
}
