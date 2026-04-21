using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.UserBookStatus;

public sealed class ReadingStatus : ValueObject
{
    public string Value { get; }
    private ReadingStatus(string value) => Value = value;
    public static readonly ReadingStatus Wishlist  = new("WISHLIST");
    public static readonly ReadingStatus Reading   = new("READING");
    public static readonly ReadingStatus Completed = new("COMPLETED");

    public static ReadingStatus From(string value) => value.ToUpper() switch
    {
        "WISHLIST"  => Wishlist,
        "READING"   => Reading,
        "COMPLETED" => Completed,
        _ => throw new DomainException($"Reading status invalid: {value}")
    };

    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}
