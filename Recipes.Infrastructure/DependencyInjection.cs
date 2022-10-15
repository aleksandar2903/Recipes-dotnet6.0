using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Abstractions.Data;
using Recipes.Application.Common.Interfaces.Authentification;
using Recipes.Infrastructure.Authentification;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
}
