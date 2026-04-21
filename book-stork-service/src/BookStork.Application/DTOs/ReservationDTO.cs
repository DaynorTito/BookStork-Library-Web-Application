using BookStork.Application.DTOs.Book;

namespace BookStork.Application.DTOs;

public sealed record ReservationDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserFullName { get; init; } = string.Empty;
    public BookSummaryDto Book { get; init; } = null!;
    public string Status { get; init; } = string.Empty;
    public DateTime ReservedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime? FulfilledAt { get; init; }
}


public sealed record CreateReservationRequest(Guid UserId, Guid BookId);
