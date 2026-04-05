using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class CategoryMapper
{
    public static CategoryEntity ToEntity(Category c) => new() { Id = c.Id.Value, Name = c.Name, Description = c.Description };
    public static Category ToDomain(CategoryEntity e)
    {
        var cat = Category.Create(e.Name, e.Description);
        typeof(Category).GetProperty("Id",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(cat, CategoryId.From(e.Id));
        return cat;
    }
}