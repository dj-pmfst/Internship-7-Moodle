using Moodle.Application.DTOs.Auth;
using Moodle.Domain.Enums;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class CourseMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public CourseMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new List<string> { "Dodaj studenta", "Objavi obavijest", "Dodaj materijal" };

                int n = options.Count;

                MenuHelper.MenuGenerator(n, "Upravljanje kolegijima", options.ToArray());

                var choice = MenuHelper.GetMenuChoice(n);

                if (choice == n)
                {
                    return;
                }

                ConsoleHelper.Continue();
            }
        }
    }
}
