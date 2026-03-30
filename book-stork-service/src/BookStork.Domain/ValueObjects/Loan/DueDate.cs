using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Loan;

public sealed record DueDate
{
    public static readonly int DefaultLoanDays = 14;
    public DateTime Value { get; }

    private DueDate(DateTime value) => Value = value;

    public static DueDate Create(DateTime date)
    {
        if (date.ToUniversalTime() <= DateTime.UtcNow)
            throw new DomainException("DueDate must be in the future");

        return new DueDate(date.ToUniversalTime());
    }

    public static DueDate FromNow(int days = 14)
    {
        if (days <= 0)
            throw new DomainException("DueDate can not be less than 0");

        return new DueDate(DateTime.UtcNow.AddDays(days));
    }

    public bool IsOverdue(DateTime? asOf = null)
        => (asOf ?? DateTime.UtcNow) > Value;

    public int DaysRemaining()
        => (int)(Value - DateTime.UtcNow).TotalDays;

    public int DaysOverdue(DateTime returnedAt)
        => returnedAt > Value
            ? (int)(returnedAt - Value).TotalDays
            : 0;

    public override string ToString() => Value.ToString("yyyy-MM-dd");
}
