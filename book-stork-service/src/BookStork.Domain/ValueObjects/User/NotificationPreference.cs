using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.User;

public sealed class NotificationPreference : ValueObject
{
    public string Value { get; }
    
    private NotificationPreference(string value)
    {
        Value = value;
    }
    
    public static readonly NotificationPreference Email = new("EMAIL");
    public static readonly NotificationPreference NoNotification = new("NO_NOTIFICATION");
    public static readonly NotificationPreference NotificationUi = new("NOTIFICATION_UI");
    
    public static NotificationPreference From(string value)
    {
        return value.ToUpper() switch
        {
            "EMAIL" => Email,
            "NO_NOTIFICATION" => NoNotification,
            "NOTIFICATION_UI" => NotificationUi,
            _ => throw new DomainException($"Invalid UserStatus: {value}")
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
