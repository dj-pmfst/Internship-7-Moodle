using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Announcement;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.DTOs.Course;
using Moodle.Application.DTOs.Material;
using Moodle.Application.Services;
using Moodle.Domain.Entities;
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
                var options = new List<string> { "Dodaj studenta", "Objavi obavijest", "Dodaj materijal" };

                int n = options.Count;

                MenuHelper.MenuGenerator(n, "Upravljanje kolegijima", options.ToArray());

                Console.Write("\nOdabir: ");
                var choice = MenuHelper.GetMenuChoice(n+1);

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
                        await AddMaterialASync();
                        break;
                    case 4:
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

            var students = await userService.GetAllStudentsAsync();
            var studentList = students.ToList();

            if (!studentList.Any())
            {
                Console.WriteLine("Nema studenata"); 
                return;
            }

            Console.WriteLine("Dostupni studenti: \n");
            for (int i = 0; i < studentList.Count; i++)
            {
                Console.WriteLine($"{studentList[i].Id}. {studentList[i].Name} - {studentList[i].Email}");
            }

            Console.Write("\nUnesite ID studenta kojeg želite dodati: ");

            int studentId;
            var validIds = studentList.Select(u => u.Id).ToHashSet();

            while (true)
            {
                studentId = InputHelper.ReadInt();
                if (validIds.Contains(studentId))
                    break;
            }

            var request = new EnrollStudentRequest
            {
                CourseId = _courseId,
                StudentId = studentId
            };

            var result = await courseService.EnrollStudentAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Uspješno dodan student ID - {studentId}");
            }
            else
            {
                result.ValidationResult.GetErrorMessages().FirstOrDefault();
            }
        }

        private async Task AddAnnouncementAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var announcementService = scope.ServiceProvider.GetRequiredService<AnnouncementService>();

            ConsoleHelper.Title("Objava obavijesti");

            var title = InputHelper.StringValid("Naslov: ");
            var text = InputHelper.StringValid("Tekst: ");

            var request = new CreateAnnouncementRequest
            {
                CourseId = _courseId,
                ProfessorId = _currentUser.UserId,
                Title = title,
                Text = text
            };

            var result = await announcementService.CreateAnnouncementAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine("Uspješno objavljeno.");
            }
            else
            {
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine(error);
                }
            }
        }

        private async Task AddMaterialASync()
        {
            using var scope = _serviceProvider.CreateScope();
            var materialService = scope.ServiceProvider.GetRequiredService<MaterialService>();

            ConsoleHelper.Title("Dodavanje materijala");

            var name = InputHelper.StringValid("Naziv: ");
            var url = InputHelper.StringValid("URL: ");

            var request = new AddMaterialRequest
            {
                CourseId = _courseId,
                Name = name,
                Url = url
            };

            var result = await materialService.AddMaterialAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine("Uspješno dodan materijal");
            }
            else
            {
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
