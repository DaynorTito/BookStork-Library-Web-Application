using BookStork.Application.BookDetails;
using BookStork.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/authors")]
[Produces("application/json")]
public sealed class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthorsController(IMediator mediator) => _mediator = mediator;
 
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllAuthorsQuery(), ct));
 
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAuthorRequest r, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateAuthorCommand(r.Name, r.Biography), ct));
}
