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


    private Book() : base(default) {}
    public Book(BookId id, string isbn, string name, List<AuthorId> authorIds, string publisher, DateOnly publishedDate, string description, int totalCopies,
        int pageCount, BookDimensions dimensions, CategoryId categoryId, BookMetadata metadata, List<BookImage> images, DateTime createdDate) : base(id)
    {
        ISBN = isbn;
        Name = name;
        AuthorIds = authorIds;
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
    }

    public string ISBN { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    
    public List<AuthorId> AuthorIds { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public BookDimensions Dimensions { get; set; }
    public CategoryId CategoryId { get; private set; } = default!;
    public BookMetadata Metadata { get; set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }
    public BookAvailabilityStatus Status { get; private set; } = default!;
    public List<BookImage> Images { get; set; }
    public IReadOnlyList<GenreId> GenreIds => _genreIds.AsReadOnly();
    
    public DateTime DateCreated { get; set; }
    
    public DateTime? UpdatedAt { get; private set; }
    

    public void UpdateDimensions(decimal height, decimal weight, decimal thickness)
    {
        Dimensions = new BookDimensions(height, weight, thickness);
    }

    public static Book Create(string isbn, string name,  List<AuthorId> authorIds, string publisher, DateOnly publishedDate, 
        string description, int pageCount, decimal height, decimal weight, decimal thickness, CategoryId categoryId,
        string language, int totalCopies, decimal averageRating, List<BookImage> images, List<GenreId> genreIds)
    { 
        var now = DateTime.UtcNow;
        var id = BookId.New();
        var dimensions = new BookDimensions(height, weight, thickness);
        var metadata = new BookMetadata(language, averageRating);
        var book = new Book(id, isbn, name, authorIds, publisher, publishedDate, description, totalCopies, pageCount,  
            dimensions, categoryId, metadata, images, now);
        
        book._genreIds.AddRange(genreIds);

        book.RaiseDomainEvent(new BookCreatedEvent(id, name, description, now));
        
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
        AuthorIds = authorIds;
        Publisher = publisher;
        PublishedDate = publishedDate;
        Description = description;
        PageCount = pageCount;
        Dimensions = new BookDimensions(height, weight, thickness);
        CategoryId = categoryId;
        Metadata = new BookMetadata(language, averageRating);
        _genreIds.Clear();
        _genreIds.AddRange(genreIds);
        _authorIds.Clear();
        _authorIds.AddRange(authorIds);
        UpdatedAt = DateTime.UtcNow;
 
        RaiseDomainEvent(new BookUpdatedEvent(Id, Name, UpdatedAt.Value));
    }
    
    public void RegisterLoan()
    {
        if (AvailableCopies <= 0)
            throw new DomainException($"There are not copies available for '{Name}'.");
 
        AvailableCopies--;
        RefreshStatus();
        UpdatedAt = DateTime.UtcNow;
    }
 
    public void RegisterReturn()
    {
        if (AvailableCopies >= TotalCopies)
            throw new DomainException("There are not copies available for the return.");
 
        AvailableCopies++;
        RefreshStatus();
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new BookReturnedEvent(Id, Name, AvailableCopies, UpdatedAt.Value));
    }
    
    public void RegisterReservation()
    {
        if (AvailableCopies > 0)
            throw new DomainException("You cannot reserve a reservation for a book with available copies.");
 
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

    private void RefreshStatus()
    {
        Status = AvailableCopies > 0
            ? BookAvailabilityStatus.Available
            : BookAvailabilityStatus.Borrowed;
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
}
