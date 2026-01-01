using Moodle.Application.DTOs.Auth;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    internal class MessageMenu
    {
        private readonly LoginResponse _currentUser;
        private readonly IServiceProvider _serviceProvider;

        public MessageMenu(LoginResponse currentUser, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            var options = new List<string> { "Nova poruka", "Moji razgovori" };

            int n = options.Count;

            MenuHelper.MenuGenerator(n, "Razgovori", options.ToArray());

            var choice = MenuHelper.GetMenuChoice(n);

            if (choice == n)
            {
                return;
            }

            ConsoleHelper.Continue();
        }
    }
}
