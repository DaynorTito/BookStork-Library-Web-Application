using BookStork.Domain.Events;
using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Author;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Category;
using BookStork.Domain.ValueObjects.Genre;

namespace BookStork.Domain.Entities;

public sealed class Book : Entity<BookId>
{
    private readonly List<GenreId> _genreIds = [];
    private readonly List<AuthorId> _authorIds = [];

    private Book() : base(default!) { }


    public Book(
        BookId id,
        string isbn,
        string name,
        List<AuthorId> authorIds,
        string publisher,
        DateOnly publishedDate,
        string description,
        int totalCopies,
        int pageCount,
        BookDimensions dimensions,
        CategoryId categoryId,
        BookMetadata metadata,
        List<BookImage> images,
        DateTime createdDate) : base(id)
    {
        ISBN = isbn;
        Name = name;
        _authorIds.AddRange(authorIds);
        Publisher = publisher;
        PublishedDate = publishedDate;
        Description = description;
        PageCount = pageCount;
        Dimensions = dimensions;
        TotalCopies = totalCopies;
        CategoryId = categoryId;
        Metadata = metadata;
        Images = images;
        DateCreated = createdDate;

        AvailableCopies = totalCopies;
        Status = BookAvailabilityStatus.Available;
    }

    public string ISBN { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;

    public IReadOnlyList<AuthorId> AuthorIds => _authorIds.AsReadOnly();

    public string Publisher { get; private set; } = string.Empty;
    public DateOnly PublishedDate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public int PageCount { get; private set; }
    public BookDimensions Dimensions { get; private set; } = default!;
    public CategoryId CategoryId { get; private set; } = default!;
    public BookMetadata Metadata { get; private set; } = default!;
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }
    public BookAvailabilityStatus Status { get; private set; } = default!;
    public List<BookImage> Images { get; private set; } = [];
    public IReadOnlyList<GenreId> GenreIds => _genreIds.AsReadOnly();
    public DateTime DateCreated { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Book Create(
        string isbn,
        string name,
        List<AuthorId> authorIds,
        string publisher,
        DateOnly publishedDate,
        string description,
        int pageCount,
        decimal height,
        decimal weight,
        decimal thickness,
        CategoryId categoryId,
        string language,
        int totalCopies,
        decimal averageRating,
        List<BookImage> images,
        List<GenreId> genreIds)
    {
        if (totalCopies < 1)
            throw new DomainException("The Book must have at least one copies");

        var id = BookId.New();
        var now = DateTime.UtcNow;

        var book = new Book(
            id, isbn, name, authorIds, publisher, publishedDate,
            description, totalCopies, pageCount,
            new BookDimensions(height, weight, thickness),
            categoryId,
            new BookMetadata(language, averageRating),
            images, now);

        book._genreIds.AddRange(genreIds);
        book.RaiseDomainEvent(new BookCreatedEvent(id, name, isbn, now));

        return book;
    }

    public static Book Rehydrate(
        BookId id,
        string isbn,
        string name,
        List<AuthorId> authorIds,
        string publisher,
        DateOnly publishedDate,
        string description,
        int totalCopies,
        int pageCount,
        BookDimensions dimensions,
        CategoryId categoryId,
        BookMetadata metadata,
        List<BookImage> images,
        List<GenreId> genreIds,
        int availableCopies,
        BookAvailabilityStatus status,
        DateTime createdDate,
        DateTime? updatedAt)
    {
        var book = new Book(
            id,
            isbn,
            name,
            authorIds,
            publisher,
            publishedDate,
            description,
            totalCopies,
            pageCount,
            dimensions,
            categoryId,
            metadata,
            images,
            createdDate
        );

        book.AvailableCopies = availableCopies;
        book.Status = status;
        book.UpdatedAt = updatedAt;

        foreach (var gid in genreIds)
            book._genreIds.Add(gid);

        return book;
    }

    public void Update(
        string name,
        List<AuthorId> authorIds,
        string publisher,
        DateOnly publishedDate,
        string description,
        int pageCount,
        decimal height,
        decimal weight,
        decimal thickness,
        CategoryId categoryId,
        string language,
        decimal averageRating,
        List<GenreId> genreIds)
    {
        Name = name;
        _authorIds.Clear();
        _authorIds.AddRange(authorIds);
        Publisher = publisher;
        PublishedDate = publishedDate;
        Description = description;
        PageCount = pageCount;
        Dimensions = new BookDimensions(height, weight, thickness);
        CategoryId = categoryId;
        Metadata = new BookMetadata(language, averageRating);
        _genreIds.Clear();
        _genreIds.AddRange(genreIds);
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new BookUpdatedEvent(Id, Name, UpdatedAt.Value));
    }

    public void RegisterLoan()
    {
        if (AvailableCopies <= 0)
            throw new DomainException($"There are not copies for book: '{Name}'.");

        AvailableCopies--;
        RefreshStatus();
        UpdatedAt = DateTime.UtcNow;
    }

    public void RegisterReturn()
    {
        if (AvailableCopies >= TotalCopies)
            throw new DomainException("There are no active loans for this book.");

        AvailableCopies++;
        RefreshStatus();
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new BookReturnedEvent(Id, Name, AvailableCopies, UpdatedAt.Value));
    }

    public void RegisterReservation()
    {
        if (AvailableCopies > 0)
            throw new DomainException("Cannot reserve a book with available copies.");

        Status = BookAvailabilityStatus.Reserved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CancelReservation()
    {
        RefreshStatus();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRating(decimal newRating)
    {
        Metadata = new BookMetadata(Metadata.Language, newRating);
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsAvailable() => AvailableCopies > 0;

    public void AddImage(string url)
    {
        Images.Add(new BookImage(url));
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveImage(string url)
    {
        Images.RemoveAll(i => i.Url == url);
        UpdatedAt = DateTime.UtcNow;
    }

    private void RefreshStatus()
    {
        Status = AvailableCopies > 0
            ? BookAvailabilityStatus.Available
            : BookAvailabilityStatus.Borrowed;
    }
}
