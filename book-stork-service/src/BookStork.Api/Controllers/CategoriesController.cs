using BookStork.Application.BookDetails;
using BookStork.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStork.Api.Controllers;

 
[ApiController]
[Route("api/v1/categories")]
[Produces("application/json")]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;
 
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(new GetAllCategoriesQuery(), ct));
 
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest r, CancellationToken ct)
        => Ok(await _mediator.Send(new CreateCategoryCommand(r.Name, r.Description), ct));
}