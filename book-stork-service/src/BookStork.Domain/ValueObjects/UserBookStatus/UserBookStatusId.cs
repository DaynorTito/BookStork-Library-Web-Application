namespace BookStork.Domain.ValueObjects.UserBookStatus;

public sealed class UserBookStatusId : ValueObject
{
    public Guid Value { get; }
    private UserBookStatusId(Guid value) => Value = value;
    public static UserBookStatusId New() => new(Guid.NewGuid());
    public static UserBookStatusId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
}
