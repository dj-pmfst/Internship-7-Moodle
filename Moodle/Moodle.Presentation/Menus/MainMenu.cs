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

                int n = options.Count;

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

                var choice = MenuHelper.GetMenuChoice(n+1);

                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        var userMenu = new CourseSelectScreen(_currentUser, _serviceProvider, options[0]);
                        await userMenu.ShowAsync();
                        break;
                    case 2:
                        var chatMenu = new MessageMenu(_currentUser, _serviceProvider);
                        await chatMenu.ShowAsync();
                        break;
                    case 3:
                        if (_currentUser.Role == Roles.profesor)
                        {
                            var courseMenu = new CourseSelectScreen(_currentUser, _serviceProvider, options[2]);
                            await courseMenu.ShowAsync();
                        }
                        else if (_currentUser.Role == Roles.admin)
                        {
                            Console.WriteLine("da");
                            var menu = new AdminMenu(_currentUser, _serviceProvider);
                            await menu.ShowAsync();
                        }
                        break;
                    case 4:
                        return;
                }
                ConsoleHelper.Continue();
            }
        }
    }
}
