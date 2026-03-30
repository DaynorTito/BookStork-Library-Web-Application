namespace BookStork.Domain.ValueObjects.Author;

public sealed class AuthorId : ValueObject
{
    public Guid Value { get; }
    private AuthorId(Guid value) => Value = value;
    public static AuthorId New() => new(Guid.NewGuid());
    public static AuthorId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(AuthorId id) => id.Value;
}
