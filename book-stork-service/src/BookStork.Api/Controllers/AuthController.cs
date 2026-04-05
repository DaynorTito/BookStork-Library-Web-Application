using BookStork.Application.DTOs;
using BookStork.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api;

[ApiController]
[Route("api/v1/auth")]
[Produces("application/json")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new RegisterUserCommand(req.Email, req.FirstName, req.LastName, req.Password, req.LoanLimit), ct);
        return CreatedAtRoute("GetUserById", new { id = result.Id }, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(req.Email, req.Password), ct);
        return Ok(result);
    }
}