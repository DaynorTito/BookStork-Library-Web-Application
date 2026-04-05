using BookStork.Api.Middleware;
using BookStork.Application.DependencyInjection;
using BookStork.Infrastructure.DependencyInjection;
using BookStork.Infrastructure.Hubs;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;
using SwaggerExtensions = BookStork.Api.Extensions.SwaggerExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
SwaggerExtensions.AddSwaggerDocumentation(builder.Services);

var app = builder.Build();

await ApplyMigrationsAsync(app);

SwaggerExtensions.UseSwaggerDocumentation(app);
app.MapHub<NotificationHub>("/hubs/notifications");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        logger.LogInformation("[DB] Apply migrations — {Env}", app.Environment.EnvironmentName);
        await db.Database.MigrateAsync();
        logger.LogInformation("[DB] Migrations completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[DB] Error applying migrations — the app will not start.");
        throw;
    }
}