namespace BookStork.Domain.ValueObjects.User;

public sealed class LoanLimit : ValueObject
{
    public int Value { get; }

    private const int MinLimit = 1;
    private const int MaxLimit = 10;

    private LoanLimit(int value)
    {
        if (value < MinLimit)
            throw new ArgumentException($"Loan Limit must be at least {MinLimit}");

        if (value > MaxLimit)
            throw new ArgumentException($"Loan Limit cannot exceed {MaxLimit}");

        Value = value;
    }

    public static LoanLimit Create(int value)
    {
        return new LoanLimit(value);
    }

    public static LoanLimit Default() => new(3);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
