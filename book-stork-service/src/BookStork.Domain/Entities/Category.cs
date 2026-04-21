using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Category;

namespace BookStork.Domain.Entities;

public sealed class Category : Entity<CategoryId>
{
    private Category() : base(default!) { }
    private Category(CategoryId id, string name, string? description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public static Category Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty.");
        return new Category(CategoryId.New(), name.Trim(), description?.Trim());
    }

    public void Update(string name, string? description)
    {
        Name = name.Trim();
        Description = description?.Trim();
    }
}
