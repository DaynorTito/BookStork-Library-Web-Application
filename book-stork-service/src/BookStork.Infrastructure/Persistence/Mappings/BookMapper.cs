using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Domain.ValueObjects.Genre;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class BookMapper
{
    public static BookEntity ToEntity(Book b) => new()
    {
        Id = b.Id.Value,
        ISBN = b.ISBN,
        Title = b.Name,
        CategoryId = b.CategoryId.Value,
        Publisher = b.Publisher,
        PublishedDate = b.PublishedDate,
        Description = b.Description,
        PageCount = b.PageCount,
        Height = b.Dimensions.Height,
        Weight = b.Dimensions.Weight,
        Thickness = b.Dimensions.Thickness,
        Language = b.Metadata.Language,
        AverageRating = b.Metadata.AverageRating,
        Status = b.Status.Value,
        TotalCopies = b.TotalCopies,
        AvailableCopies = b.AvailableCopies,
        CreatedAt = b.DateCreated,
        UpdatedAt = b.UpdatedAt,
        Images = b.Images.Select((img, i) => new BookImageEntity
        {
            Id = Guid.NewGuid(),
            BookId = b.Id.Value,
            Url = img.Url,
            IsPrimary = i == 0
        }).ToList(),
        BookGenres = b.GenreIds.Select(gid => new BookGenreEntity
        {
            BookId = b.Id.Value,
            GenreId = gid.Value
        }).ToList(),
        BookAuthors = b.AuthorIds.Select(aid => new BookAuthorEntity
        {
            BookId = b.Id.Value,
            AuthorId = aid.Value
        }).ToList()
    };

    public static Book ToDomain(BookEntity e)
    {
        var images = e.Images
            .OrderByDescending(i => i.IsPrimary)
            .Select(i => new BookImage(i.Url))
            .ToList();

        var authorIds = e.BookAuthors
            .Select(ba => AuthorId.From(ba.AuthorId))
            .ToList();

        var genreIds = e.BookGenres
            .Select(bg => GenreId.From(bg.GenreId))
            .ToList();

        return Book.Rehydrate(
            BookId.From(e.Id),
            e.ISBN,
            e.Title,
            authorIds,
            e.Publisher,
            e.PublishedDate,
            e.Description,
            e.TotalCopies,
            e.PageCount,
            new BookDimensions(e.Height, e.Weight, e.Thickness),
            CategoryId.From(e.CategoryId),
            new BookMetadata(e.Language, e.AverageRating),
            images,
            genreIds,
            e.AvailableCopies,
            BookAvailabilityStatus.From(e.Status),
            e.CreatedAt,
            e.UpdatedAt
        );
    }

    public static BookDetailDto ToDetailDto(BookEntity e) => new()
    {
        Id          = e.Id,
        ISBN        = e.ISBN,
        Title       = e.Title,
        Authors     = e.BookAuthors.Select(ba => new AuthorDto
        {
            Id        = ba.Author.Id,
            Name      = ba.Author.Name,
            Biography = ba.Author.Biography
        }).ToList(),
        Category    = new CategoryDto
        {
            Id          = e.Category.Id,
            Name        = e.Category.Name,
            Description = e.Category.Description
        },
        Genres      = e.BookGenres.Select(bg => new GenreDto
        {
            Id   = bg.Genre.Id,
            Name = bg.Genre.Name
        }).ToList(),
        Publisher    = e.Publisher,
        PublishedDate = e.PublishedDate,
        Description  = e.Description,
        PageCount    = e.PageCount,
        Height       = e.Height,
        Weight       = e.Weight,
        Thickness    = e.Thickness,
        Language     = e.Language,
        AverageRating = e.AverageRating,
        Status       = e.Status,
        AvailableCopies = e.AvailableCopies,
        TotalCopies  = e.TotalCopies,
        Images       = e.Images.OrderByDescending(i => i.IsPrimary).Select(i => i.Url).ToList(),
        CreatedAt    = e.CreatedAt,
        UpdatedAt    = e.UpdatedAt
    };

    public static void Update(BookEntity e, Book b)
    {
        e.Title = b.Name;
        e.CategoryId = b.CategoryId.Value;
        e.Publisher = b.Publisher;
        e.PublishedDate = b.PublishedDate;
        e.Description = b.Description;
        e.PageCount = b.PageCount;
        e.Height = b.Dimensions.Height;
        e.Weight = b.Dimensions.Weight;
        e.Thickness = b.Dimensions.Thickness;
        e.Language = b.Metadata.Language;
        e.AverageRating = b.Metadata.AverageRating;
        e.Status = b.Status.Value;
        e.AvailableCopies = b.AvailableCopies;
        e.UpdatedAt = b.UpdatedAt;
        e.BookAuthors = b.AuthorIds.Select(aid => new BookAuthorEntity
        {
            BookId = b.Id.Value,
            AuthorId = aid.Value
        }).ToList();
        e.BookGenres = b.GenreIds.Select(gid => new BookGenreEntity
        {
            BookId = b.Id.Value,
            GenreId = gid.Value
        }).ToList();
    }

    private static void SetPrivate(object obj, string prop, object? val)
        => obj.GetType()
              .GetProperty(prop,
                  System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
              ?.SetValue(obj, val);
}