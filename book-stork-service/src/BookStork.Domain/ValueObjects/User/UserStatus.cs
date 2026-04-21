namespace BookStork.Domain.ValueObjects.User;

public class UserStatus : ValueObject
{
    public string Value { get; }
    
    private UserStatus(string value)
    {
        Value = value;
    }
    
    public static readonly UserStatus Active = new("ACTIVE");
    public static readonly UserStatus Suspended = new("SUSPENDED");
    public static readonly UserStatus Expired = new("EXPIRED");
    
    public static UserStatus From(string value)
    {
        return value.ToUpper() switch
        {
            "ACTIVE" => Active,
            "SUSPENDED" => Suspended,
            "EXPIRED" => Expired,
            _ => throw new ArgumentException($"Invalid UserStatus: {value}")
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
