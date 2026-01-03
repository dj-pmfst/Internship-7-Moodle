using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Announcement;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.DTOs.Course;
using Moodle.Application.DTOs.Material;
using Moodle.Application.Services;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class CourseMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly int _courseId;
        private readonly IServiceProvider _serviceProvider;

        public CourseMenu(LoginResponse currentUser, int courseId, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _courseId = courseId;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new[] { "Dodaj studenta", "Objavi obavijest", "Dodaj materijal" };
                int choice = KeyboardHelper.MenuGeneratorWithHybridInput(options.Length, "Upravljanje kolegijima", options);

                switch (choice)
                {
                    case 0: 
                        Environment.Exit(0);
                        break;
                    case 1: 
                        await AddStudentAsync(); 
                        break;
                    case 2: 
                        await AddAnnouncementAsync(); 
                        break;
                    case 3: 
                        await AddMaterialAsync(); 
                        break;
                    case -1: 
                        return;
                }

                ConsoleHelper.Continue();
            }
        }

        private async Task AddStudentAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Dodavanje studenta");
            var students = (await userService.GetAllStudentsAsync()).ToList();
            if (!students.Any()) 
            { 
                Console.WriteLine("Nema studenata"); 
                return; 
            }

            var student = MenuHelper.SelectFromList(students, "Odaberite studenta", s => $"{s.Name} - {s.Email}");
            if (student == null) 
                return;

            var result = await courseService.EnrollStudentAsync(new EnrollStudentRequest
            {
                CourseId = _courseId,
                StudentId = student.Id
            });

            ConsoleHelper.DisplayResult(result, $"Uspješno dodan student ID - {student.Id}");
        }

        private async Task AddAnnouncementAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var announcementService = scope.ServiceProvider.GetRequiredService<AnnouncementService>();

            ConsoleHelper.Title("Objava obavijesti");

            var title = InputHelper.StringValid("Naslov: ");
            var text = InputHelper.StringValid("Tekst: ");

            var result = await announcementService.CreateAnnouncementAsync(new CreateAnnouncementRequest
            {
                CourseId = _courseId,
                ProfessorId = _currentUser.UserId,
                Title = title,
                Text = text
            });

            ConsoleHelper.DisplayResult(result, "Uspješno objavljeno.");
        }

        private async Task AddMaterialAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var materialService = scope.ServiceProvider.GetRequiredService<MaterialService>();

            ConsoleHelper.Title("Dodavanje materijala");

            var name = InputHelper.StringValid("Naziv: ");
            var url = InputHelper.StringValid("URL: ");

            var result = await materialService.AddMaterialAsync(new AddMaterialRequest
            {
                CourseId = _courseId,
                Name = name,
                Url = url
            });

            ConsoleHelper.DisplayResult(result, "Uspješno dodan materijal");
        }
    }
}
