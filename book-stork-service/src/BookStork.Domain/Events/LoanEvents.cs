using BookStork.Domain.Abstractions;
using BookStork.Domain.ValueObjects.Book;
using BookStork.Domain.ValueObjects.Loan;
using BookStork.Domain.ValueObjects.User;

namespace BookStork.Domain.Events;

public sealed record LoanCreatedEvent(LoanId LoanId, UserId UserId, BookId BookId, DueDate DueDate, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record LoanReturnedEvent(LoanId LoanId, UserId UserId, BookId BookId, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
 
public sealed record LoanOverdueEvent(LoanId LoanId, UserId UserId, BookId BookId, DueDate DueDate, DateTime OccurredOn) : IDomainEvent
{ public Guid EventId { get; } = Guid.NewGuid(); }
