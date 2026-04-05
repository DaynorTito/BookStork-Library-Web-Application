using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class UserMapper
{
    public static UserEntity ToEntity(User u) => new()
    {
        Id = u.Id.Value, Email = u.Email.Value,
        FirstName = u.FirstName, LastName = u.LastName,
        PasswordHash = u.PasswordHash,
        Status = u.Status.Value, LoanLimit = u.LoanLimit.Value,
        NotificationPreference = u.NotificationPreference.Value,
        CreatedAt = u.CreatedAt, UpdatedAt = u.UpdatedAt
    };
 
    public static User ToDomain(UserEntity e)
    {
        var user = (User)Activator.CreateInstance(typeof(User),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new object[] {
                UserId.Create(e.Id), Email.Create(e.Email),
                e.FirstName, e.LastName, e.PasswordHash,
                UserStatus.From(e.Status), LoanLimit.Create(e.LoanLimit),
                NotificationPreference.From(e.NotificationPreference), e.CreatedAt
            }, null)!;
 
        if (e.UpdatedAt.HasValue)
            SetPrivate(user, "UpdatedAt", e.UpdatedAt);
 
        return user;
    }
 
    public static void Update(UserEntity e, User u)
    {
        e.Email = u.Email.Value; e.FirstName = u.FirstName; e.LastName = u.LastName;
        e.PasswordHash = u.PasswordHash; e.Status = u.Status.Value;
        e.LoanLimit = u.LoanLimit.Value;
        e.NotificationPreference = u.NotificationPreference.Value;
        e.UpdatedAt = u.UpdatedAt;
    }
 
    private static void SetPrivate(object obj, string prop, object? val)
        => obj.GetType().GetProperty(prop,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(obj, val);
}