using BookStork.Application.Ports;
using Microsoft.Extensions.Logging;

namespace BookStork.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public Task SendWelcomeAsync(string toEmail, string fullName, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Welcome → {FullName} <{Email}>", fullName, toEmail);
        return Task.CompletedTask;
    }

    public Task SendLoanConfirmationAsync(string toEmail, string bookTitle, DateTime dueDate, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Loan Confirmed → <{Email}> book='{Book}' due={DueDate:yyyy-MM-dd}",
            toEmail, bookTitle, dueDate);
        return Task.CompletedTask;
    }

    public Task SendLoanOverdueAsync(string toEmail, string bookTitle, DateTime dueDate, CancellationToken ct = default)
    {
        _logger.LogWarning("[Email] Loan Overdue → <{Email}> book='{Book}' overdue={DueDate:yyyy-MM-dd}",
            toEmail, bookTitle, dueDate);
        return Task.CompletedTask;
    }

    public Task SendReservationAvailableAsync(string toEmail, string bookTitle, CancellationToken ct = default)
    {
        _logger.LogInformation("[Email] Reservation Available → <{Email}> book='{Book}'", toEmail, bookTitle);
        return Task.CompletedTask;
    }
}