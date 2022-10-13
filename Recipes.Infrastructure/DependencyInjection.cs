using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Application.Services.Authentification;
using Recipes.Infrastructure.Authentification;

namespace Recipes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddScoped<IJwtTokenGenerator, IJwtTokenGenerator>();
        return services;
    }
}
