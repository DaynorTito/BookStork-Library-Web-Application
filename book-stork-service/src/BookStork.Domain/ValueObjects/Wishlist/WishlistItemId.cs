namespace BookStork.Domain.ValueObjects.Wishlist;

public sealed class WishlistItemId : ValueObject
{
    public Guid Value { get; }
    private WishlistItemId(Guid value) => Value = value;
    public static WishlistItemId New() => new(Guid.NewGuid());
    public static WishlistItemId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(WishlistItemId id) => id.Value;
}