using BookStork.Application.Books;
using BookStork.Application.Common;
using BookStork.Application.Common.Mappings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BookStork.Application.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceExtensions).Assembly;
 
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(assembly);
 
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
            cfg.AddProfile<BookMappingProfile>();
            cfg.AddProfile<LoanMappingProfile>();
            cfg.AddProfile<ReservationMappingProfile>();
            cfg.AddProfile<UserBookStatusMappingProfile>();
        });
        services.AddScoped<BookEnricher>();

        return services;
    }
}
