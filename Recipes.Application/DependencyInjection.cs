using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Behaviours;
using System.Reflection;

namespace Recipes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
