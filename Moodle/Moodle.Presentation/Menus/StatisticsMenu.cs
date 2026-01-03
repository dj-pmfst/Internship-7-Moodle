using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class StatisticsMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public StatisticsMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new List<string> {"Danas", "Ovaj mjesec", "Ukupno"};

                var choice = KeyboardHelper.MenuGeneratorWithHybridInput(options.Count(), "Statistika", options.ToArray());

                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        await ShowDailyStatisticsAsync();
                        break;
                    case 2:
                        await ShowMonthlyStatisticsAsync();
                        break;
                    case 3:
                        await ShowTotalStatisticsAsync();
                        break;
                    case -1:
                        return;
                }
            }
        }

        private async Task ShowDailyStatisticsAsync()
        {
            var today = DateTime.UtcNow.Date;
            await ShowStatisticsAsync("Danas", today);
        }

        private async Task ShowMonthlyStatisticsAsync()
        {
            var monthStart = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1,
                0, 0, 0,
                DateTimeKind.Utc
            );

            await ShowStatisticsAsync("Ovaj mjesec", monthStart);
        }

        private async Task ShowTotalStatisticsAsync()
        {
            await ShowStatisticsAsync("Ukupno", null);
        }

        private async Task ShowStatisticsAsync(string title, DateTime? fromDate)
        {
            Console.Clear();

            using var scope = _serviceProvider.CreateScope();
            var statisticsService = scope.ServiceProvider.GetRequiredService<StatisticsService>();

            ConsoleHelper.Title(title);

            var stats = await statisticsService.GetStatisticsAsync(fromDate);

            Console.WriteLine("\n------Korisnici po ulogama------\n");
            Console.WriteLine($"Studenti:   {stats.TotalStudents}");
            Console.WriteLine($"Profesori:  {stats.TotalProfessors}");
            Console.WriteLine($"Admini:     {stats.TotalAdmins}");

            Console.WriteLine("\n------Kolegiji------\n");
            Console.WriteLine($"Ukupno: {stats.TotalCourses}");

            Console.WriteLine("\n------Top 3 kolegija po broju studenata------\n");
            if (stats.TopCourses.Any())
            {
                for (int i = 0; i < stats.TopCourses.Count; i++)
                {
                    var c = stats.TopCourses[i];
                    Console.WriteLine($"{i + 1}. {c.CourseName} - {c.StudentCount} studenata");
                }
            }
            else
            {
                Console.WriteLine("Nema podataka");
            }

            Console.WriteLine("\n------Top 3 korisnika po broju poruka------\n");
            if (stats.TopMessageSenders.Any())
            {
                for (int i = 0; i < stats.TopMessageSenders.Count; i++)
                {
                    var u = stats.TopMessageSenders[i];
                    var name = u.UserName;
                    if (u.UserName == null)
                    {
                        name = "Izbrisan korisnik";
                    }
                    Console.WriteLine($"{i + 1}. {name} - {u.MessageCount} poruka");
                }
            }
            else
            {
                Console.WriteLine("Nema podataka");
            }

            ConsoleHelper.Continue();
        }
    }
}