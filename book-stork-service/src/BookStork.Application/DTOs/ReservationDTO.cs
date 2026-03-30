namespace BookStork.Application.DTOs;

public sealed record ReservationDto(
    Guid Id,
    Guid UserId,
    string UserFullName,
    Guid BookId,
    string BookTitle,
    string Status,
    DateTime ReservedAt,
    DateTime ExpiresAt,
    DateTime? FulfilledAt);
 
public sealed record CreateReservationRequest(Guid UserId, Guid BookId);
