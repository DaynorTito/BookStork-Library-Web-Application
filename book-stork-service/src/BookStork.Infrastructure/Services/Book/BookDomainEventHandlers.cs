using BookStork.Application.Common;
using BookStork.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services.Book;

public sealed class BookCreatedAuditHandler
    : INotificationHandler<DomainEventWrapper<BookCreatedEvent>>
{
    private readonly ILogger<BookCreatedAuditHandler> _logger;

    public BookCreatedAuditHandler(ILogger<BookCreatedAuditHandler> logger)
        => _logger = logger;

    public Task Handle(
        DomainEventWrapper<BookCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[Audit][BookCreated] BookId={BookId} Name={Name} OccurredOn={OccurredOn}",
            evt.IdBook, evt.name, evt.OccurredOn);

        // 
        // await _auditRepository.LogAsync(new AuditLog { ... });
        // await _messageBus.PublishAsync(new BookCreatedIntegrationEvent(evt.BookId));

        return Task.CompletedTask;
    }
}

public sealed class BookUpdatedAuditHandler
    : INotificationHandler<DomainEventWrapper<BookUpdatedEvent>>
{
    private readonly ILogger<BookUpdatedAuditHandler> _logger;

    public BookUpdatedAuditHandler(ILogger<BookUpdatedAuditHandler> logger)
        => _logger = logger;

    public Task Handle(
        DomainEventWrapper<BookUpdatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[Audit][BookUpdated] BookId={BookId} Name={Name} OccurredOn={OccurredOn}",
            evt.BookId, evt.Name, evt.OccurredOn);

        return Task.CompletedTask;
    }
}

public sealed class BookDeletedAuditHandler
    : INotificationHandler<DomainEventWrapper<BookDeletedEvent>>
{
    private readonly ILogger<BookDeletedAuditHandler> _logger;

    public BookDeletedAuditHandler(ILogger<BookDeletedAuditHandler> logger)
        => _logger = logger;

    public Task Handle(
        DomainEventWrapper<BookDeletedEvent> notification,
        CancellationToken cancellationToken)
    {
        var evt = notification.DomainEvent;

        _logger.LogInformation(
            "[Audit][BookDeleted] BookId={BookId} OccurredOn={OccurredOn}",
            evt.BookId, evt.OccurredOn);

        return Task.CompletedTask;
    }
}
