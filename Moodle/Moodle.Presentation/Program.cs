using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moodle.Application;
using Moodle.Application.Services;
using Moodle.Infrastructure;
using Moodle.Infrastructure.Persistence;
using Moodle.Presentation.Menus;

namespace Moodle.Presentation
{
    public class Program
    {
        private static IServiceProvider? _serviceProvider;

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            _serviceProvider = host.Services;

            await InitializeDatabaseAsync();
            await RunApplicationAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>  
                {
                    services.AddInfrastructure(context.Configuration);
                    services.AddApplication();
                });

        static async Task InitializeDatabaseAsync()
        {
            using var scope = _serviceProvider!.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MoodleDbContext>();

            await context.Database.EnsureCreatedAsync();
            await DataSeed.SeedAsync(context);
        }

        static async Task RunApplicationAsync()
        {
            while (true)
            {
                using var scope = _serviceProvider!.CreateScope();
                var authService = scope.ServiceProvider.GetRequiredService<AuthenticationService>();

                var authMenu = new AuthenticationMenu(authService);
                var currentUser = await authMenu.ShowAsync();

                if (currentUser == null)
                {
                    Console.WriteLine("Izlazak iz aplikacije");
                    return;
                }

                var mainMenu = new MainMenu(currentUser, _serviceProvider);
                await mainMenu.ShowAsync();
            }
        }
    }
}
