using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
using Moodle.Domain.Enums;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class UserMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly int _courseId;
        private readonly IServiceProvider _serviceProvider;

        public UserMenu(LoginResponse currentUser, int courseId, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _courseId = courseId;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();
            var courseName = await courseService.GetCourseNameAsync(_courseId) ?? "Nepoznat kolegij";

            while (true)
            {
                var options = new List<string> { "Obavijesti", "Materijali" };

                if (_currentUser.Role == Roles.profesor)
                {
                    options.Add("Studenti");
                }

                int n = options.Count;

                MenuHelper.MenuGenerator(n, $"Kolegij '{courseName}'", options.ToArray());
                n = n + 1;
                var choice = MenuHelper.GetMenuChoice(n);

                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        await GetAnnouncementsAsync();
                        break;
                    case 2:
                        await GetMaterialsAsync();
                        break;
                    case 3:
                        if (_currentUser.Role == Roles.profesor)
                        {
                            await GetStudentsAsync();
                            break;
                        }
                        else
                        {
                            return;
                        }   
                     case 4:
                        return;
                }
                ConsoleHelper.Continue();
            }
        }

        private async Task GetAnnouncementsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var announcementService = scope.ServiceProvider.GetRequiredService<AnnouncementService>();

            ConsoleHelper.Title("Obavijesti");

            var announcements = await announcementService.GetByCourseAsync(_courseId);
            var announcementList = announcements.ToList();

            if (!announcementList.Any())
            {
                Console.WriteLine("Nema obavijesti."); ;
            }
            else
            {
                foreach (var announcement in announcementList)
                {
                    Console.WriteLine($"\n[{announcement.FormattedDate}] {announcement.Professor}");
                    Console.WriteLine($"Title: {announcement.Title}");
                    Console.WriteLine($"Text: {announcement.Text}");
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        private async Task GetMaterialsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var materialsService = scope.ServiceProvider.GetRequiredService<MaterialService>();

            ConsoleHelper.Title("Materijali");

            var materials = await materialsService.GetByCourseAsync(_courseId);
            var materialList = materials.ToList();

            if (!materialList.Any())
            {
                Console.WriteLine("Nema materijala.");
            }
            else
            {
                foreach (var material in materials)
                {
                    Console.WriteLine($"\n[{material.FormattedDate}]");
                    Console.WriteLine($"Naziv: {material.Name}");
                    Console.WriteLine($"URL: {material.Url}");
                    Console.WriteLine(new string('-', 50));
                }
            }
        }

        private async Task GetStudentsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();

            ConsoleHelper.Title("Studenti");

            var students = await courseService.GetCourseStudentsAsync(_courseId);
            var studentList = students.OrderBy(s => s.Name).ToList();

            if (!studentList.Any())
            {
                Console.WriteLine("Nema studenata."); 
            }
            else
            {
                foreach (var student in studentList)
                {
                    Console.WriteLine($"-({student.Name}) ({student.Email})");
                }
            }
        }
    }   
}
