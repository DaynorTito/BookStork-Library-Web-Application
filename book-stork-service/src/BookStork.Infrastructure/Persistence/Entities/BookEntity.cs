namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class BookEntity
{
    public Guid Id { get; set; }
    public string ISBN { get; set; } = null!;
    public string Title { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public string Publisher { get; set; } = null!;
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; } = null!;
    public int PageCount { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public string Language { get; set; } = null!;
    public decimal AverageRating { get; set; }
    public string Status { get; set; } = null!;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public CategoryEntity Category { get; set; } = null!;
    public List<BookImageEntity> Images { get; set; } = [];
    public List<BookGenreEntity> BookGenres { get; set; } = [];
    
    public List<BookAuthorEntity> BookAuthors { get; set; } = [];

    public List<LoanEntity> Loans { get; set; } = [];
    public List<ReservationEntity> Reservations { get; set; } = [];
}
