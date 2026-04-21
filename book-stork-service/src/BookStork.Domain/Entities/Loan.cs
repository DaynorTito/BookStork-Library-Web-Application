using BookStork.Domain.Events;
using BookStork.Domain.Exceptions;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Entities;

public sealed class Loan : Entity<LoanId>
{
    private Loan() : base(default!) { }

    private Loan(LoanId id, UserId userId, BookId bookId, DueDate dueDate, DateTime loanedAt) : base(id)
    {
        UserId = userId;
        BookId = bookId;
        DueDate = dueDate;
        Status = LoanStatus.Active;
        LoanedAt = loanedAt;
    }

    public UserId UserId { get; private set; } = default!;
    public BookId BookId { get; private set; } = default!;
    public DueDate DueDate { get; private set; } = default!;
    public LoanStatus Status { get; private set; } = default!;
    public DateTime LoanedAt { get; private set; }
    public DateTime? ReturnedAt { get; private set; }

    public static Loan Create(UserId userId, BookId bookId, int days = 14)
    {
        var id = LoanId.New();
        var now = DateTime.UtcNow;
        var loan = new Loan(id, userId, bookId, DueDate.FromNow(days), now);
        loan.RaiseDomainEvent(new LoanCreatedEvent(id, userId, bookId, loan.DueDate, now));
        return loan;
    }

    public void Return()
    {
        if (Status == LoanStatus.Returned)
            throw new DomainException("This loan has already been returned.");

        ReturnedAt = DateTime.UtcNow;
        Status = LoanStatus.Returned;

        RaiseDomainEvent(new LoanReturnedEvent(Id, UserId, BookId, ReturnedAt.Value));
    }

    public bool IsOverdue() => Status == LoanStatus.Active && DueDate.IsOverdue();

    public void MarkOverdue()
    {
        if (Status != LoanStatus.Active)
            throw new DomainException("Only active loans can be marked as overdue.");
        Status = LoanStatus.Overdue;
        RaiseDomainEvent(new LoanOverdueEvent(Id, UserId, BookId, DueDate, DateTime.UtcNow));
    }
}
