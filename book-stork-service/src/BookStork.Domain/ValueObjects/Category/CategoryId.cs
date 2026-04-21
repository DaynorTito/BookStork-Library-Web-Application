namespace BookStork.Domain.ValueObjects.Category;

public sealed class CategoryId : ValueObject
{
    public Guid Value { get; }
    private CategoryId(Guid value) => Value = value;
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(CategoryId id) => id.Value;
}
