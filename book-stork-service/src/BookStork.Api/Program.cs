using BookStork.Api.Middleware;
using BookStork.Application.DependencyInjection;
using BookStork.Infrastructure.DependencyInjection;
using BookStork.Infrastructure.Hubs;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;
using SwaggerExtensions = BookStork.Api.Extensions.SwaggerExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var origins = builder.Configuration
            .GetSection("AllowedOrigins")
            .Get<string[]>()
            ?? ["http://localhost:5173"];

        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    SwaggerExtensions.AddSwaggerDocumentation(builder.Services);
}

var app = builder.Build();

await ApplyMigrationsAsync(app);

if (app.Environment.IsDevelopment())
{
    SwaggerExtensions.UseSwaggerDocumentation(app);
}

app.MapHub<NotificationHub>("/hubs/notifications");

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

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
        logger.LogInformation("[DB] Applying migrations — {Env}", app.Environment.EnvironmentName);

        var retries = 5;
        while (retries > 0)
        {
            try
            {
                await db.Database.MigrateAsync();
                logger.LogInformation("[DB] Migrations completed successfully.");
                break;
            }
            catch (Exception ex) when (retries > 1)
            {
                retries--;
                logger.LogWarning("[DB] DB not ready, retrying in 3s... ({Retries} left). Error: {Msg}",
                    retries, ex.Message);
                await Task.Delay(3000);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[DB] Fatal error applying migrations — app will not start.");
        throw;
    }
}
