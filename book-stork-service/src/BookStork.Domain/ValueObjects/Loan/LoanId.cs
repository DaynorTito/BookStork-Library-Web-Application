using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Loan;

public sealed class LoanId : ValueObject
{
    public Guid Value { get; }

    private LoanId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("LoanId cannot be empty");
        Value = value;
    }
    
    public static LoanId Create(Guid value)
    {
        return new LoanId(value);
    }

    public static LoanId New()
    {
        return new LoanId(Guid.NewGuid());
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
