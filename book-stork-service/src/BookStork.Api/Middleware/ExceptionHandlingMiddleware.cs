using System.Net;
using System.Text.Json;
using BookStork.Domain.Exceptions;
using FluentValidation;
namespace BookStork.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
 
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
 
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }
 
    private async Task HandleAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, errors) = exception switch
        {
            ValidationException vex => (
                HttpStatusCode.BadRequest,
                "Error de validación",
                (object?)vex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())
            ),
            NotFoundException nex => (
                HttpStatusCode.NotFound,
                nex.Message,
                (object?)null
            ),
            ConflictException cex => (
                HttpStatusCode.Conflict,
                cex.Message,
                (object?)null
            ),
            DomainException dex => (
                HttpStatusCode.UnprocessableEntity,
                dex.Message,
                (object?)null
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Ocurrió un error interno en el servidor.",
                (object?)null
            )
        };
 
        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Error no controlado: {Message}", exception.Message);
        else
            _logger.LogWarning("[{Code}] {Message}", (int)statusCode, exception.Message);
 
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;
 
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(new
            {
                type   = $"https://httpstatuses.io/{(int)statusCode}",
                title,
                status = (int)statusCode,
                traceId = context.TraceIdentifier,
                errors
            }, JsonOptions));
    }
}