namespace BookStork.Application.DTOs;

public sealed record AuthorDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Biography { get; init; }
}

public sealed record GenreDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

    public sealed record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
} 
public sealed record CreateAuthorRequest(string Name, string? Biography);
public sealed record CreateGenreRequest(string Name);
public sealed record CreateCategoryRequest(string Name, string? Description);
