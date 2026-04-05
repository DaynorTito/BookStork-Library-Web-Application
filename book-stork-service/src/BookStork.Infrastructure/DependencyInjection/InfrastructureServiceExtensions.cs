using System.Text;
using BookStork.Application.Books;
using BookStork.Application.Loans;
using BookStork.Application.Ports;
using BookStork.Application.Reservations;
using BookStork.Application.Wishlists;
using BookStork.Domain.Repositories;
using BookStork.Infrastructure.Persistence.Queries;
using BookStork.Infrastructure.Persistence.Repositories;
using BookStork.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace BookStork.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                }));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserBookStatusRepository, UserBookStatusRepository>();

        services.AddScoped<ILoanQueryService, LoanQueryService>();
        services.AddScoped<IReservationQueryService, ReservationQueryService>();
        services.AddScoped<IWishlistQueryService, WishlistQueryService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<INotificationService, SignalRNotificationService>();
        services.AddScoped<IBookQueryService, BookQueryService>();

        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]
                            ?? throw new InvalidOperationException("Jwt:Secret not configured."))),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


        services.AddAuthorization();

        return services;
    }
}
