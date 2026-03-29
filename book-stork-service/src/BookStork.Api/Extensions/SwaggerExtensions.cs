using Microsoft.OpenApi.Models;

namespace BookStork.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "BookStork API",
                Version     = "v1",
                Description = "API con Arquitectura Hexagonal + DDD"
            });
        });
 
        return services;
    }
 
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStork API v1");
                c.RoutePrefix = string.Empty;
                c.DisplayRequestDuration();
            });
        }
 
        return app;
    }
}