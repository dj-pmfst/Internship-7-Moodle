using Moodle.Application.DTOs.Auth;
using Moodle.Domain.Enums;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class MainMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public MainMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                var options = new List<string> { "Kolegiji", "Razgovori" };

                int n = 2;

                if (_currentUser.Role == Roles.profesor)
                {
                    options.Add("Upravljanje kolegijima");
                    n++;
                }
                else if (_currentUser.Role == Roles.admin)
                {
                    options.Add("Korisnici");
                    n ++;
                }

                MenuHelper.MenuGenerator(n,"Glavni izbornik", options.ToArray());

                var choice = MenuHelper.GetMenuChoice(options.Count);

                if (choice == options.Count)
                {
                    return;
                }

                ConsoleHelper.Continue();
            }
        }
    }
}
