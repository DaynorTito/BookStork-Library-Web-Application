using BookStork.Api.Extensions;
using BookStork.Api.Middleware;
using BookStork.Application.DependencyInjection;
using BookStork.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddApplicationServices();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSwaggerDocumentation();


var app = builder.Build();

await ApplyMigrationsAsync(app);

app.UseSwaggerDocumentation();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider
        .GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
 
        logger.LogInformation(
            "[DB] Aplicando migraciones — ambiente: {Env}",
            app.Environment.EnvironmentName);

        await db.Database.MigrateAsync();
 
        logger.LogInformation("[DB] Migraciones OK.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[DB] Error al migrar. La app no iniciará.");
        throw;
    }
}