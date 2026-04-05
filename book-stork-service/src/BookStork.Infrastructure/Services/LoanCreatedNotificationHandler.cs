using BookStork.Application.Common;
using BookStork.Application.Ports;
using BookStork.Domain.Events;
using BookStork.Domain.Repositories;
using MediatR;

namespace BookStork.Infrastructure.Services;

public sealed class LoanCreatedNotificationHandler : INotificationHandler<DomainEventWrapper<LoanCreatedEvent>>
{
    private readonly IEmailService _email;
    private readonly IUserRepository _userRepo;
    private readonly IBookRepository _bookRepo;
 
    public LoanCreatedNotificationHandler(IEmailService email, IUserRepository userRepo, IBookRepository bookRepo)
    { _email = email; _userRepo = userRepo; _bookRepo = bookRepo; }
 
    public async Task Handle(DomainEventWrapper<LoanCreatedEvent> n, CancellationToken ct)
    {
        var evt = n.DomainEvent;
        var user = await _userRepo.GetByIdAsync(evt.UserId, ct);
        var book = await _bookRepo.GetByIdAsync(evt.BookId, ct);
        if (user is not null && book is not null)
            await _email.SendLoanConfirmationAsync(user.Email.Value, book.Name, evt.DueDate.Value, ct);
    }
}