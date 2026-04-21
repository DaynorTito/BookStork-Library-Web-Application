using MediatR;
using Microsoft.Extensions.Logging;
using BookStork.Application.Common;
using BookStork.Domain.Events;


namespace BookStork.Application.Books.EventHandlers;

public sealed class BookCreatedEventHandler
    : INotificationHandler<DomainEventWrapper<BookCreatedEvent>>
{
    private readonly IMediator _mediator;
    private readonly ILogger<BookCreatedEventHandler> _logger;

    public BookCreatedEventHandler(IMediator mediator, ILogger<BookCreatedEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(
        DomainEventWrapper<BookCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[BookCreated] BookId={BookId} Name={ISBN}",
            evt.IdBook, evt.name);

        // 
        // await _mediator.Send(new NotifyNewBookCommand(evt.BookId, evt.Name), ct);
        // await _mediator.Send(new IndexBookForSearchCommand(evt.BookId), ct);

        return Task.CompletedTask;
    }
}

public sealed class BookUpdatedEventHandler
    : INotificationHandler<DomainEventWrapper<BookUpdatedEvent>>
{
    private readonly ILogger<BookUpdatedEventHandler> _logger;

    public BookUpdatedEventHandler(ILogger<BookUpdatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(
        DomainEventWrapper<BookUpdatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[BookUpdated] BookId={BookId} Name={Name}",
            evt.BookId, evt.Name);

        // await _mediator.Send(new InvalidateBookCacheCommand(evt.BookId), ct);

        return Task.CompletedTask;
    }
}

public sealed class BookDeletedEventHandler
    : INotificationHandler<DomainEventWrapper<BookDeletedEvent>>
{
    private readonly ILogger<BookDeletedEventHandler> _logger;

    public BookDeletedEventHandler(ILogger<BookDeletedEventHandler> logger)
        => _logger = logger;

    public Task Handle(
        DomainEventWrapper<BookDeletedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[BookDeleted] BookId={BookId}",
            evt.BookId);

        return Task.CompletedTask;
    }
}
