using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.Services;

namespace Moodle.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationService>();
            services.AddScoped<UserService>();
            services.AddScoped<CourseService>();
            services.AddScoped<MaterialService>();
            services.AddScoped<MessageService>();
            services.AddScoped<AnnouncementService>();

            return services;
        }
    }
}
