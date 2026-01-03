using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.DTOs.Course;
using Moodle.Application.Services;
using Moodle.Domain.Enums;
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

            while (true)
            {
                var courses = await LoadCoursesAsync(courseService);

                if (!courses.Any())
                {
                    Console.WriteLine("Nema dostupnih kolegija.");
                    return; 
                }

                var selectedCourse = SelectCourse(courses);

                if (selectedCourse == null)
                    return; 

                await OpenNextMenuAsync(selectedCourse.Id);
            }
        }

        private async Task<List<CourseDTO>> LoadCoursesAsync(CourseService courseService)
        {
            return _currentUser.Role switch
            {
                Roles.admin => (await courseService.GetAllCoursesAsync()).ToList(),
                _ => (await courseService.GetCoursesByStudentAsync(_currentUser.UserId)).ToList()
            };
        }

        private CourseDTO? SelectCourse(List<CourseDTO> courses)
        {
            MenuHelper.MenuGenerator(
                courses.Count,
                "Odaberite kolegij",
                courses.Select(c => c.Name).ToArray()
            );

            int maxChoice = courses.Count + 1;
            int choice = MenuHelper.GetMenuChoice(maxChoice);

            if (choice == 0)
                Environment.Exit(0);

            if (choice == maxChoice)
                return null; 

            return courses[choice - 1];
        }

        private async Task OpenNextMenuAsync(int courseId)
        {
            if (_choice == "Kolegiji")
            {
                var userMenu = new UserMenu(_currentUser, courseId, _serviceProvider);
                await userMenu.ShowAsync();
            }
            else
            {
                var courseMenu = new CourseMenu(_currentUser, courseId, _serviceProvider);
                await courseMenu.ShowAsync();
            }
        }
    }
}

