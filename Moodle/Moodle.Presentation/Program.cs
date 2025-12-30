using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moodle.Application;
using Moodle.Infrastructure;
using Moodle.Infrastructure.Persistence;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation
{
    public class Program
    {
        private static IServiceProvider? _serviceProvider;
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build;
            _serviceProvider = host.Services;

            //Fix
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructure(context.Configuration);
                services.AddApplication();
            });

        //ADD seeeds

        static async Task InitializeDataBaseAsync()
        {
            using var scope = _serviceProvider!.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MoodleDbContext>();

            await context.Database.EnsureCreatedAsync();

            await SeedDataAsync(context);
        }

        static async SeedDataAsnyc(MoodleDbContext context)
        {

        }

        static async Task RunApplicationAsync()
        {
            using var scope = _serviceProvider!.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MoodleDbContext>();

            Console.WriteLine("Pokrenuta aplikacija");
            ConsoleHelper.Continue();
        }
    }
}
