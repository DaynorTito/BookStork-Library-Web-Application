namespace BookStork.Application.DTOs;

public sealed record LoanDto(
    Guid Id,
    Guid UserId,
    string UserFullName,
    Guid BookId,
    string BookTitle,
    string Status,
    DateTime LoanedAt,
    DateTime DueDate,
    DateTime? ReturnedAt,
    bool IsOverdue,
    int DaysRemaining);
 
public sealed record CreateLoanRequest(Guid UserId, Guid BookId, int Days = 14);
