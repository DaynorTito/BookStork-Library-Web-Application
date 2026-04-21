using BookStork.Domain.Entities;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;
using BookStork.Infrastructure.Persistence.Entities;

namespace BookStork.Infrastructure.Persistence.Mappings;

public static class LoanMapper
{
    public static LoanEntity ToEntity(Loan l) => new()
    {
        Id = l.Id.Value, UserId = l.UserId.Value, BookId = l.BookId.Value,
        Status = l.Status.Value, LoanedAt = l.LoanedAt,
        DueDate = l.DueDate.Value, ReturnedAt = l.ReturnedAt
    };
 
    public static Loan ToDomain(LoanEntity e)
    {
        var loan = (Loan)Activator.CreateInstance(typeof(Loan),
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            null,
            new object[] {
                LoanId.Create(e.Id), UserId.Create(e.UserId),
                BookId.From(e.BookId), DueDate.Create(e.DueDate), e.LoanedAt
            }, null)!;
 
        SetPrivate(loan, "Status", LoanStatus.From(e.Status));
        if (e.ReturnedAt.HasValue) SetPrivate(loan, "ReturnedAt", e.ReturnedAt);
        return loan;
    }
 
    public static void Update(LoanEntity e, Loan l)
    {
        e.Status = l.Status.Value; e.ReturnedAt = l.ReturnedAt;
    }
 
    private static void SetPrivate(object obj, string prop, object? val)
        => obj.GetType().GetProperty(prop,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
            ?.SetValue(obj, val);
}