using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.User;

public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("UserId cannot be empty");

        Value = value;
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }

    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
