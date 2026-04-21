using BookStork.Application.DTOs;
using BookStork.Application.DTOs.Book;
using BookStork.Domain.Entities;
using BookStork.Domain.Repositories;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Domain.ValueObjects.Genre;

namespace BookStork.Application.Books;

public sealed class BookEnricher
{
    private readonly IAuthorRepository _authorRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IGenreRepository _genreRepo;

    public BookEnricher(
        IAuthorRepository authorRepo,
        ICategoryRepository categoryRepo,
        IGenreRepository genreRepo)
    {
        _authorRepo = authorRepo;
        _categoryRepo = categoryRepo;
        _genreRepo = genreRepo;
    }

    public async Task<BookDetailDto> EnrichAsync(
        Book book,
        CancellationToken ct = default)
    {
        var authorTasks = book.AuthorIds
            .Select(id => _authorRepo.GetByIdAsync(id, ct));

        var categoryTask = _categoryRepo.GetByIdAsync(book.CategoryId, ct);

        var genreTasks = book.GenreIds
            .Select(id => _genreRepo.GetByIdAsync(id, ct));

        var authors = (await Task.WhenAll(authorTasks))
            .Where(a => a is not null)
            .Select(a => new AuthorDto
            {
                Id = a!.Id.Value,
                Name = a.Name,
                Biography = a.Biography
            })
            .ToList();

        var category = await categoryTask;
        var categoryDto = category is not null
            ? new CategoryDto
            {
                Id = category.Id.Value,
                Name = category.Name,
                Description = category.Description
            }
            : new CategoryDto
            {
                Id = Guid.Empty,
                Name = string.Empty,
                Description = null
            };
        var genres = (await Task.WhenAll(genreTasks))
            .Where(g => g is not null)
            .Select(g => new GenreDto
            {
                Id = g!.Id.Value,
                Name = g.Name
            })
            .ToList();

        return new BookDetailDto
        {
            Id = book.Id.Value,
            ISBN = book.ISBN,
            Title = book.Name,
            Authors = authors,
            Category = categoryDto,
            Genres = genres,
            Publisher = book.Publisher,
            PublishedDate = book.PublishedDate,
            Description = book.Description,
            PageCount = book.PageCount,
            Height = book.Dimensions.Height,
            Weight = book.Dimensions.Weight,
            Thickness = book.Dimensions.Thickness,
            Language = book.Metadata.Language,
            AverageRating = book.Metadata.AverageRating,
            Status = book.Status.Value,
            AvailableCopies = book.AvailableCopies,
            TotalCopies = book.TotalCopies,
            Images = book.Images.Select(i => i.Url).ToList(),
            CreatedAt = book.DateCreated,
            UpdatedAt = book.UpdatedAt
        };
    }


    public async Task<IReadOnlyList<BookDetailDto>> EnrichManyAsync(
        IReadOnlyList<Book> books,
        CancellationToken ct = default)
    {
        if (books.Count == 0) return [];

        var allAuthorIds = books.SelectMany(b => b.AuthorIds).Distinct().ToList();
        var allCategoryIds = books.Select(b => b.CategoryId).Distinct().ToList();
        var allGenreIds = books.SelectMany(b => b.GenreIds).Distinct().ToList();

        var authorsByIdTask = LoadAuthorsAsync(allAuthorIds, ct);
        var categoriesByIdTask = LoadCategoriesAsync(allCategoryIds, ct);
        var genresByIdTask = LoadGenresAsync(allGenreIds, ct);

        await Task.WhenAll(authorsByIdTask, categoriesByIdTask, genresByIdTask);

        var authorsById = await authorsByIdTask;
        var categoriesById = await categoriesByIdTask;
        var genresById = await genresByIdTask;

        return books.Select(book =>
        {
            var authors = book.AuthorIds
                .Where(id => authorsById.ContainsKey(id.Value))
                .Select(id => authorsById[id.Value])
                .ToList();

            var categoryDto = categoriesById.TryGetValue(book.CategoryId.Value, out var cat)
                ? cat
                : new CategoryDto
                {
                    Id = Guid.Empty,
                    Name = string.Empty,
                    Description = null
                };

            var genres = book.GenreIds
                .Where(id => genresById.ContainsKey(id.Value))
                .Select(id => genresById[id.Value])
                .ToList();

            return new BookDetailDto
            {
                Id = book.Id.Value,
                ISBN = book.ISBN,
                Title = book.Name,
                Authors = authors,
                Category = categoryDto,
                Genres = genres,
                Publisher = book.Publisher,
                PublishedDate = book.PublishedDate,
                Description = book.Description,
                PageCount = book.PageCount,
                Height = book.Dimensions.Height,
                Weight = book.Dimensions.Weight,
                Thickness = book.Dimensions.Thickness,
                Language = book.Metadata.Language,
                AverageRating = book.Metadata.AverageRating,
                Status = book.Status.Value,
                AvailableCopies = book.AvailableCopies,
                TotalCopies = book.TotalCopies,
                Images = book.Images.Select(i => i.Url).ToList(),
                CreatedAt = book.DateCreated,
                UpdatedAt = book.UpdatedAt
            };
        }).ToList().AsReadOnly();
    }

    private async Task<Dictionary<Guid, AuthorDto>> LoadAuthorsAsync(
        IEnumerable<AuthorId> ids, CancellationToken ct)
    {
        var tasks = ids.Select(id => _authorRepo.GetByIdAsync(id, ct));
        var results = await Task.WhenAll(tasks);
        return results
            .Where(a => a is not null)
            .ToDictionary(
                a => a!.Id.Value,
                a => new AuthorDto
                {
                    Id = a.Id.Value,
                    Name = a.Name,
                    Biography = a.Biography
                });
    }

    private async Task<Dictionary<Guid, CategoryDto>> LoadCategoriesAsync(
        IEnumerable<CategoryId> ids, CancellationToken ct)
    {
        var tasks = ids.Select(id => _categoryRepo.GetByIdAsync(id, ct));
        var results = await Task.WhenAll(tasks);
        return results
            .Where(c => c is not null)
            .ToDictionary(
                c => c!.Id.Value,
                c => new CategoryDto
                {
                    Id = c.Id.Value,
                    Name = c.Name,
                    Description = c.Description
                });
    }

    private async Task<Dictionary<Guid, GenreDto>> LoadGenresAsync(
        IEnumerable<GenreId> ids, CancellationToken ct)
    {
        var tasks = ids.Select(id => _genreRepo.GetByIdAsync(id, ct));
        var results = await Task.WhenAll(tasks);
        return results
            .Where(g => g is not null)
            .ToDictionary(
                g => g!.Id.Value,
                g => new GenreDto
                {
                    Id = g.Id.Value,
                    Name = g.Name
                });
    }
}
