using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.Services;

namespace Moodle.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationService>();

            return services;
        }
    }
}
