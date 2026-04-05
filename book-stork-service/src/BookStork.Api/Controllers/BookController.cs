using BookStork.Application.Books.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookStork.Application.Books.Queries;
using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using Microsoft.AspNetCore.Authorization;

namespace BookStork.API.Controllers;


[ApiController]
[Route("api/v1/books")]
[Produces("application/json")]
public sealed class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    public BooksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<BookListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFromQuery([FromQuery] BookFilterRequest f, CancellationToken ct)
        => Ok(await _mediator.Send(
            new GetFilteredBooksQuery(f.Title, f.AuthorId, f.GenreId, f.CategoryId,
                f.Keyword, f.Language, f.SortBy, f.Ascending, f.Page, f.PageSize), ct));

    [HttpGet("all")]
    [ProducesResponseType(typeof(PagedResult<BookListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _mediator.Send(
            new GetAllBooksQuery(), ct));


    [HttpGet("{id:guid}", Name = "GetBookById")]
    [ProducesResponseType(typeof(BookDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetBookByIdQuery(id), ct));

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest r, CancellationToken ct)
    {
        var id = await _mediator.Send(new CreateBookCommand(
            r.ISBN, r.Title, r.AuthorIds, r.CategoryId, r.GenreIds, r.Publisher,
            r.PublishedDate, r.Description, r.PageCount, r.Height, r.Weight,
            r.Thickness, r.Language, r.AverageRating, r.TotalCopies, r.Images), ct);
        return CreatedAtRoute("GetBookById", new { id }, id);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(BookDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest r, CancellationToken ct)
        => Ok(await _mediator.Send(new UpdateBookCommand(
            id, r.Title, r.AuthorIds, r.CategoryId, r.GenreIds, r.Publisher,
            r.PublishedDate, r.Description, r.PageCount, r.Height, r.Weight, r.Thickness, r.Language, r.AverageRating), ct));

    [HttpDelete("{id:guid}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        await _mediator.Send(new DeleteBookCommand(id), ct);
        return NoContent();
    }

    [HttpPost("reading-status")]
    [Authorize]
    [ProducesResponseType(typeof(UserBookStatusDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SetReadingStatus([FromBody] SetUserBookStatusRequest r, CancellationToken ct)
        => Ok(await _mediator.Send(new SetUserBookStatusCommand(r.UserId, r.BookId, r.Status), ct));
}