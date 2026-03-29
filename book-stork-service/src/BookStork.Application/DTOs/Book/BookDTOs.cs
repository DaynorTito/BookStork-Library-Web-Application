namespace BookStork.Application.DTOs.Book;

public class CreateBookDTO
{
    public string ISBN  { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public DateOnly PublishedDate { get; set; }
    public string Description { get; set; }
    public int PageCount { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public decimal Thickness { get; set; }
    public decimal AverageRating { get; set; }
    public string Language { get; set; }
    public List<string> Images { get; set; }
}
