using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Services.Authentification;

namespace Recipes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthentificationService, AuthentificationService>();
            return services;
        }
    }
}
