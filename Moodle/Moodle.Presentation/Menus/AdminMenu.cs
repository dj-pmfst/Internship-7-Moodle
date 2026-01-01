using Moodle.Application.DTOs.Auth;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class AdminMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public AdminMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            var options = new List<string> { "Brisanje korisnika", "Uređivanje emaila", "Promjena uloge" };

            int n = options.Count;

            MenuHelper.MenuGenerator(n, "Upravljanje korisnicima", options.ToArray());

            var choice = MenuHelper.GetMenuChoice(n);

            if (choice == n)
            {
                return;
            }

            ConsoleHelper.Continue();
        }
    }
}
