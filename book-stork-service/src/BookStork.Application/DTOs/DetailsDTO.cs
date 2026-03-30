namespace BookStork.Application.DTOs;

public sealed record AuthorDto(Guid Id, string Name, string? Biography);
public sealed record GenreDto(Guid Id, string Name);
public sealed record CategoryDto(Guid Id, string Name, string? Description);
 
public sealed record CreateAuthorRequest(string Name, string? Biography);
public sealed record CreateGenreRequest(string Name);
public sealed record CreateCategoryRequest(string Name, string? Description);
