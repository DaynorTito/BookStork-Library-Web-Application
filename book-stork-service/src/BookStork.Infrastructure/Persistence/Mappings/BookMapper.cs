using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class BookMapper
{
    public static BookEntity ToEntity(Book book) => new()
    {
        Id = book.Id,
        ISBN = book.ISBN,
        Name = book.Name,
        Author = book.Author,
        Publisher = book.Publisher,
        PublishedDate = book.PublishedDate,
        Description = book.Description,
        PageCount = book.PageCount,
        Height = book.Dimensions.Height,
        Weight = book.Dimensions.Weight,
        Thickness = book.Dimensions.Thickness,
        CategoryId = book.CategoryId,
        AverageRating = book.Metadata.AverageRating,
        Language = book.Metadata.Language,
        CreatedAt = book.DateCreated,
        UpdatedAt = book.UpdatedAt,
        Images = book.Images.Select(i => new BookImageEntity
        {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            Url = i.Url,
            IsPrimary = book.Images.IndexOf(i) == 0
        }).ToList()
    };
 
    public static Book ToDomain(BookEntity entity)
    {
        var images = entity.Images
            .Select(i => new BookImage(i.Url))
            .ToList();

        var book = new Book(
            entity.Id,
            entity.ISBN,
            entity.Name,
            entity.Author,
            entity.Publisher,
            entity.PublishedDate,
            entity.Description,
            entity.PageCount,
            new BookDimensions(entity.Height, entity.Weight, entity.Thickness),
            entity.CategoryId,
            new BookMetadata(entity.Language, entity.AverageRating),
            images,
            entity.CreatedAt);
        if (entity.UpdatedAt.HasValue)
        {
            typeof(Book)
                .GetProperty(nameof(Book.UpdatedAt),
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance)
                ?.SetValue(book, entity.UpdatedAt);
        }
        return book;
    }
 
    public static void UpdateEntity(BookEntity entity, Book book)
    {
        entity.Name = book.Name;
        entity.Author = book.Author;
        entity.Publisher = book.Publisher;
        entity.PublishedDate = book.PublishedDate;
        entity.Description = book.Description;
        entity.PageCount = book.PageCount;
        entity.Height = book.Dimensions.Height;
        entity.Weight = book.Dimensions.Weight;
        entity.Thickness = book.Dimensions.Thickness;
        entity.CategoryId = book.CategoryId;
        entity.AverageRating = book.Metadata.AverageRating;
        entity.Language = book.Metadata.Language;
        entity.UpdatedAt = book.UpdatedAt;
 
        entity.Images = book.Images.Select(i => new BookImageEntity
        {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            Url = i.Url,
            IsPrimary = book.Images.IndexOf(i) == 0
        }).ToList();
    }
}
