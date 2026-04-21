using BookStork.Application.DTOs;
using BookStork.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Produces("application/json")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsersController(IMediator mediator) => _mediator = mediator;
 
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
        => Ok(await _mediator.Send(new GetAllUsersQuery(page, pageSize), ct));
 
    [HttpGet("{id:guid}", Name = "GetUserById")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserByIdQuery(id), ct));
 
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserRequest req, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateUserCommand(id, req.Email, req.FirstName, req.LastName, req.NotificationPreference), ct));
 
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteUserCommand(id), ct);
        return NoContent();
    }
}