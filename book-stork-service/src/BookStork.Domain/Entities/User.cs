using BookStork.Domain.Events;
using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Entities;

public class User : Entity<UserId>
{
    private readonly List<LoanId> _activeLoans = [];

    private User(
        UserId id,
        Email email,
        string firstName,
        string lastName,
        string passwordHash,
        UserStatus status,
        LoanLimit loanLimit,
        NotificationPreference notificationPreference,
        DateTime createdAt) : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Status = status;
        LoanLimit = loanLimit;
        NotificationPreference = notificationPreference;
        CreatedAt = createdAt;
    }
    
    private User() : base(default) {}

    public Email Email { get; private set; } = default!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserStatus Status { get; private set; } = default!;
    public LoanLimit LoanLimit { get; private set; } = default!;
    public NotificationPreference NotificationPreference { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public IReadOnlyList<LoanId> ActiveLoans => _activeLoans.AsReadOnly();
    
    
    public static User Create(
        string email,
        string firstName,
        string lastName,
        string passwordHash,
        int loanLimit = 5)
    {
        var id = UserId.New();
        var now = DateTime.UtcNow;
 
        var user = new User(
            id,
            Email.Create(email),
            firstName.Trim(),
            lastName.Trim(),
            passwordHash,
            UserStatus.Active,
            LoanLimit.Create(loanLimit),
            NotificationPreference.Email,
            now);
 
        user.RaiseDomainEvent(new UserCreatedEvent(id, user.Email, now));
        return user;
    }
    
    public void UpdateProfile(string firstName, string lastName, string email, string notificationPreference)
    {
        if (Status == UserStatus.Suspended)
            throw new DomainException("You cannot change the profile status");
 
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Email = Email.Create(email);
        NotificationPreference = NotificationPreference.From(notificationPreference);
        UpdatedAt = DateTime.UtcNow;
 
        RaiseDomainEvent(new UserUpdatedEvent(Id, Email, UpdatedAt.Value));
    }
 
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }
 
    public void Suspend()
    {
        if (Status == UserStatus.Suspended)
            throw new DomainException("User already suspended");
        Status = UserStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new UserSuspendedEvent(Id, UpdatedAt.Value));
    }
 
    public void Activate()
    {
        if (Status == UserStatus.Active)
            throw new DomainException("User already active");
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
 
    public void AddActiveLoan(LoanId loanId)
    {
        if (Status != UserStatus.Active)
            throw new DomainException("Just active users can ask for loans.");
 
        if (_activeLoans.Count >= LoanLimit.Value)
            throw new DomainException(
                $"User reach limit permited {LoanLimit.Value} for simultaneously loans.");
 
        if (_activeLoans.Contains(loanId))
            throw new DomainException("Loan already registered.");
 
        _activeLoans.Add(loanId);
    }
 
    public void RemoveActiveLoan(LoanId loanId)
    {
        if (!_activeLoans.Remove(loanId))
            throw new DomainException("Loan not registered for user.");
    }
 
    public bool CanBorrow() =>
        Status == UserStatus.Active && _activeLoans.Count < LoanLimit.Value;
}
