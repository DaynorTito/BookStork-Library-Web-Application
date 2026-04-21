using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Book;

 
public sealed class BookAvailabilityStatus : ValueObject
{
    public string Value { get; }
    private BookAvailabilityStatus(string value) => Value = value;
    public static readonly BookAvailabilityStatus Available = new("AVAILABLE");
    public static readonly BookAvailabilityStatus Borrowed  = new("BORROWED");
    public static readonly BookAvailabilityStatus Reserved  = new("RESERVED");
 
    public static BookAvailabilityStatus From(string value) => value.ToUpper() switch
    {
        "AVAILABLE" => Available,
        "BORROWED"  => Borrowed,
        "RESERVED"  => Reserved,
        _ => throw new DomainException($"Estado de libro inválido: {value}")
    };
 
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}
