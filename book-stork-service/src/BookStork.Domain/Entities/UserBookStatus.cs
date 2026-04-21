using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.User;
using BookStork.Domain.ValueObjects.UserBookStatus;

namespace BookStork.Domain.Entities;

public sealed class UserBookStatus : Entity<UserBookStatusId>
{
    private UserBookStatus() : base(default!) { }
 
    private UserBookStatus(UserBookStatusId id, UserId userId, BookId bookId, ReadingStatus status, DateTime updatedAt)
        : base(id)
    {
        UserId = userId;
        BookId = bookId;
        Status = status;
        UpdatedAt = updatedAt;
    }
 
    public UserId UserId { get; private set; } = default!;
    public BookId BookId { get; private set; } = default!;
    public ReadingStatus Status { get; private set; } = default!;
    public DateTime UpdatedAt { get; private set; }
 
    public static UserBookStatus Create(UserId userId, BookId bookId, string status)
        => new(UserBookStatusId.New(), userId, bookId, ReadingStatus.From(status), DateTime.UtcNow);
 
    public void ChangeStatus(string newStatus)
    {
        Status = ReadingStatus.From(newStatus);
        UpdatedAt = DateTime.UtcNow;
    }
}
