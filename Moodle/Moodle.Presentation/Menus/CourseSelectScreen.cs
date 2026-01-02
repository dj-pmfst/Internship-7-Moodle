using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class CourseSelectScreen
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _choice;

        public CourseSelectScreen(LoginResponse currentUser, IServiceProvider serviceProvider, string choice)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
            _choice = choice;
        }

        public async Task ShowAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var courseService = scope.ServiceProvider.GetRequiredService<CourseService>();

            var courses = await courseService.GetCoursesByStudentAsync(_currentUser.UserId);
            var courseList = courses.ToList();

            if (!courseList.Any())
            {
                Console.WriteLine("Nema upisanih kolegija.");
                ConsoleHelper.Continue();
                return;
            }

            MenuHelper.MenuGenerator(
                courseList.Count,
                "Odaberite kolegij",
                courseList.Select(c => c.Name).ToArray()
            );

            int choice = MenuHelper.GetMenuChoice(courseList.Count);

            var selectedCourse = courseList[choice - 1];

            if (_choice != "Ipravljanje kolegijima")
            {
            var userMenu = new UserMenu(_currentUser, selectedCourse.Id, _serviceProvider);
            await userMenu.ShowAsync();
            }
            else
            {
                var courseMenu = new CourseMenu(_currentUser, selectedCourse.Id, _serviceProvider);
                await courseMenu.ShowAsync();
            }

        }
    }
}
