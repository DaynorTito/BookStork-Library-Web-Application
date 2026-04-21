namespace BookStork.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string Status,
    int LoanLimit,
    int ActiveLoansCount,
    string NotificationPreference,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
 
public sealed record CreateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    int LoanLimit = 3,
    string NotificationPreference = "EMAIL");
 
public sealed record UpdateUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string NotificationPreference);
    