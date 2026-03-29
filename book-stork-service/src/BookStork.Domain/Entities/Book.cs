using BookStork.Domain.Events;
using BookStork.Domain.ValueObjects.Book;

namespace BookStork.Domain.Entities;

public sealed class Book : Entity<Guid>
{
    private Book() : base(default) {}
    public Book(Guid id, string isbn, string name, string author, string publisher, DateOnly publishedDate, string description, 
        int pageCount, BookDimensions dimensions, Guid categoryId, BookMetadata metadata, List<BookImage> images, DateTime createdDate) : base(id)
    {
        ISBN = isbn;
        Name = name;
        Author = author;
        Publisher = publisher;
        PublishedDate = publishedDate;
        Description = description;
        PageCount = pageCount;
        Dimensions = dimensions;
        CategoryId = categoryId;
        Metadata = metadata;
        Images = images;
        DateCreated = createdDate;
    }

    public string ISBN { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public BookDimensions Dimensions { get; set; }
    public Guid CategoryId { get; set; }
    public BookMetadata Metadata { get; set; }
    public List<BookImage> Images { get; set; }
    
    public DateTime DateCreated { get; set; }
    
    public DateTime? UpdatedAt { get; private set; }
    

    public void UpdateDimensions(decimal height, decimal weight, decimal thickness)
    {
        Dimensions = new BookDimensions(height, weight, thickness);
    }

    public static Book Create(string isbn, string name, string author, string publisher, DateOnly publishedDate, 
        string description, int pageCount, decimal height, decimal weight, decimal thickness, Guid categoryId,
        string language, decimal averageRating, List<BookImage> images)
    { 
        var now = DateTime.UtcNow;
        var id = Guid.NewGuid();
        var dimensions = new BookDimensions(height, weight, thickness);
        var metadata = new BookMetadata(language, averageRating);
        var book = new Book(id, isbn, name, author, publisher, publishedDate, description, pageCount, dimensions, categoryId, metadata, images, now);
        
        book.RaiseDomainEvent(new BookCreatedEvent(id, name, description, now));
        
        return book;
    }
    
    public void Update(
        string name,
        string author,
        string publisher,
        DateOnly publishedDate,
        string description,
        int pageCount,
        decimal height,
        decimal weight,
        decimal thickness,
        Guid categoryId,
        string language,
        decimal averageRating)
    {
        Name = name;
        Author = author;
        Publisher = publisher;
        PublishedDate = publishedDate;
        Description = description;
        PageCount = pageCount;
        Dimensions = new BookDimensions(height, weight, thickness);
        CategoryId = categoryId;
        Metadata = new BookMetadata(language, averageRating);
        UpdatedAt = DateTime.UtcNow;
 
        RaiseDomainEvent(new BookUpdatedEvent(Id, Name, UpdatedAt.Value));
    }
 
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
