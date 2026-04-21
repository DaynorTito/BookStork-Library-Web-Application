namespace BookStork.Application.DTOs;

public sealed record UserBookStatusDto(Guid UserId, Guid BookId, string BookTitle, string Status, DateTime UpdatedAt);
public sealed record SetUserBookStatusRequest(Guid UserId, Guid BookId, string Status);
