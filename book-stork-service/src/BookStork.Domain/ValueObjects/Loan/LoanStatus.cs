using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Loan;

public sealed class LoanStatus : ValueObject
{
    public string Value { get; }
    private LoanStatus(string value) => Value = value;
    public static readonly LoanStatus Active   = new("ACTIVE");
    public static readonly LoanStatus Returned = new("RETURNED");
    public static readonly LoanStatus Overdue  = new("OVERDUE");
 
    public static LoanStatus From(string value) => value.ToUpper() switch
    {
        "ACTIVE"   => Active,
        "RETURNED" => Returned,
        "OVERDUE"  => Overdue,
        _ => throw new DomainException($"Estado de préstamo inválido: {value}")
    };
 
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}
