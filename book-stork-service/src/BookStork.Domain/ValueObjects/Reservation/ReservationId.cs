namespace BookStork.Domain.ValueObjects.Reservation;

public sealed class ReservationId : ValueObject
{
    public Guid Value { get; }
    private ReservationId(Guid value) => Value = value;
    public static ReservationId New() => new(Guid.NewGuid());
    public static ReservationId From(Guid value) => new(value);
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value.ToString();
    public static implicit operator Guid(ReservationId id) => id.Value;
}
