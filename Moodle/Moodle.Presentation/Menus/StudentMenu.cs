using Moodle.Application.DTOs.Auth;
using Moodle.Domain.Enums;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class UserMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public UserMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new List<string> { "Obavijesti", "Materijali" };

                int n = options.Count;

                if (_currentUser.Role == Roles.profesor)
                {
                    options.Add("Student");
                    n++;
                }

                MenuHelper.MenuGenerator(n, "Kolegij", options.ToArray());

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
