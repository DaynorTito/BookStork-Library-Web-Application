using BookStork.Application.Common;
using BookStork.Application.Common.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace BookStork.Application.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationServiceExtensions).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
      //  services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventBehavior<,>));
 
        services.AddValidatorsFromAssembly(assembly);       
        services.AddAutoMapper(cfg => cfg.AddProfile<BookMappingProfile>());
        // AggregateTracker — Scoped (una instancia por request HTTP)
       // services.AddScoped<IAggregateTracker, AggregateTracker>();
        return services;
    }
}
