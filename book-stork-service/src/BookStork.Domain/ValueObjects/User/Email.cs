namespace BookStork.Domain.ValueObjects.User;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");

        if (!IsValidEmail(value))
            throw new ArgumentException($"Invalid email format: {value}");

        Value = value.ToLowerInvariant();
    }

    public static Email Create(string value)
    {
        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
