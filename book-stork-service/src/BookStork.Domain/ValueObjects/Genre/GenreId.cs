namespace BookStork.Domain.ValueObjects.Genre;

public sealed class GenreId : ValueObject
{
    public Guid Value { get; }
    private GenreId(Guid value) => Value = value;
    public static GenreId New() => new(Guid.NewGuid());
    public static GenreId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(GenreId id) => id.Value;
}
