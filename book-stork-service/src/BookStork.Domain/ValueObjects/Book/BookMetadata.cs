using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Book;

public class BookMetadata : ValueObject
{
    public decimal AverageRating { get; }
    public string Language { get; }

    public BookMetadata(string language, decimal averageRating)
    {
        if (string.IsNullOrWhiteSpace(language))
            throw new DomainException("Language is null or whitespace");
 
        if (averageRating < 0 || averageRating > 5)
            throw new DomainException("Rating must be between 0 and 5");

        Language = language;
        AverageRating = averageRating;
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AverageRating;
        yield return Language;
    }
}
