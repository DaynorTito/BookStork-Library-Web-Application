using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Reservation;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class ReservationMapper
{
    public static ReservationEntity ToEntity(Reservation r) => new()
    {
        Id = r.Id.Value, UserId = r.UserId.Value, BookId = r.BookId.Value,
        Status = r.Status.Value, ReservedAt = r.ReservedAt,
        ExpiresAt = r.ExpiresAt, FulfilledAt = r.FulfilledAt
    };
 
    public static Reservation ToDomain(ReservationEntity e)
    {
        var res = (Reservation)Activator.CreateInstance(typeof(Reservation),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new object[] {
                ReservationId.From(e.Id), UserId.Create(e.UserId),
                BookId.From(e.BookId), e.ReservedAt, e.ExpiresAt
            }, null)!;
 
        SetPrivate(res, "Status", ReservationStatus.From(e.Status));
        if (e.FulfilledAt.HasValue) SetPrivate(res, "FulfilledAt", e.FulfilledAt);
        return res;
    }
 
    public static void Update(ReservationEntity e, Reservation r)
    {
        e.Status = r.Status.Value; e.FulfilledAt = r.FulfilledAt;
    }
 
    private static void SetPrivate(object obj, string prop, object? val)
        => obj.GetType().GetProperty(prop,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(obj, val);
}
