using BookStork.Domain.Exceptions;

namespace BookStork.Domain.ValueObjects.Reservation;

public sealed class ReservationStatus : ValueObject
{
    public string Value { get; }
    private ReservationStatus(string value) => Value = value;
    public static readonly ReservationStatus Pending   = new("PENDING");
    public static readonly ReservationStatus Fulfilled = new("FULFILLED");
    public static readonly ReservationStatus Cancelled = new("CANCELLED");
    public static readonly ReservationStatus Expired   = new("EXPIRED");
 
    public static ReservationStatus From(string value) => value.ToUpper() switch
    {
        "PENDING"   => Pending,
        "FULFILLED" => Fulfilled,
        "CANCELLED" => Cancelled,
        "EXPIRED"   => Expired,
        _ => throw new DomainException($"Estado de reserva inválido: {value}")
    };
 
    protected override IEnumerable<object?> GetEqualityComponents() { yield return Value; }
    public override string ToString() => Value;
}
