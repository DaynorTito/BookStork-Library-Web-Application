namespace BookStork.Domain.ValueObjects.Book;

public sealed class BookId : ValueObject
{
    public Guid Value { get; }
    private BookId(Guid value) => Value = value;
    public static BookId New() => new(Guid.NewGuid());
    public static BookId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(BookId id) => id.Value;
}
