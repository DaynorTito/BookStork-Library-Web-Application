using BookStork.Domain.Abstractions;
using BookStork.Domain.Entities;

namespace BookStork.Application.Ports;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken = default);
}

public interface IEmailService
{
    Task SendWelcomeAsync(string toEmail, string fullName, CancellationToken ct = default);
    Task SendLoanConfirmationAsync(string toEmail, string bookTitle, DateTime dueDate, CancellationToken ct = default);
    Task SendLoanOverdueAsync(string toEmail, string bookTitle, DateTime dueDate, CancellationToken ct = default);
    Task SendReservationAvailableAsync(string toEmail, string bookTitle, CancellationToken ct = default);
}
 
public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string fullName);
    bool ValidateToken(string token, out Guid userId);
}
 
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}
