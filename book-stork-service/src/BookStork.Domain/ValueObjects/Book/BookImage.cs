using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Book;

public class BookImage : ValueObject
{
    public string Url { get; }

    public BookImage(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("URL should not be empty");

        Url = url.Trim();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Url;
    }
}
