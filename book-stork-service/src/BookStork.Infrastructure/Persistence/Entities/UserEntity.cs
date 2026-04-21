namespace BookStork.Infrastructure.Persistence.Entities;

public sealed class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int LoanLimit { get; set; }
    public string NotificationPreference { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<LoanEntity> Loans { get; set; } = [];
    public List<ReservationEntity> Reservations { get; set; } = [];
    public List<UserBookStatusEntity> BookStatuses { get; set; } = [];
}
