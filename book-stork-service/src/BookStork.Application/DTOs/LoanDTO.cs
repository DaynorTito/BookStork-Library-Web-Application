using BookStork.Application.DTOs.Book;

namespace BookStork.Application.DTOs;

public sealed class LoanDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string UserFullName { get; init; } = string.Empty;
    public BookSummaryDto Book { get; init; } = null!;
    public string Status { get; init; } = string.Empty;
    public DateTime LoanedAt { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ReturnedAt { get; init; }
    public bool IsOverdue { get; init; }
    public int DaysRemaining { get; init; }
}
public sealed record CreateLoanRequest(Guid UserId, Guid BookId, int Days = 14);
