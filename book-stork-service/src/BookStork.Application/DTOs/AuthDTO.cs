namespace BookStork.Application.DTOs;

public sealed record LoginRequest(string Email, string Password);
 
public sealed record AuthResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAt,
    UserDto User);
    