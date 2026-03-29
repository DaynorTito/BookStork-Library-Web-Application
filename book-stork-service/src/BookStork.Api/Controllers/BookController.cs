using BookStork.Application.Books.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookStork.Application.Books.Queries;
using BookStork.Application.DTOs.Book;

namespace BookStork.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public sealed class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ListBookDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAllBooksQuery(page, pageSize),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BookDetailDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetBookByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookDTO request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateBookCommand(
            request.ISBN,
            request.Name,
            request.Author,
            request.Publisher,
            request.PublishedDate,
            request.Description,
            request.PageCount,
            request.CategoryId,
            request.Height,
            request.Weight,
            request.Thickness,
            request.AverageRating,
            request.Language,
            request.Images);

        var id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BookDetailDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateBookDTO request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateBookCommand(
            id,
            request.Name,
            request.Author,
            request.Publisher,
            request.PublishedDate,
            request.Description,
            request.PageCount,
            request.CategoryId,
            request.Height,
            request.Weight,
            request.Thickness,
            request.AverageRating,
            request.Language);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteBookCommand(id), cancellationToken);
        return NoContent();
    }
}