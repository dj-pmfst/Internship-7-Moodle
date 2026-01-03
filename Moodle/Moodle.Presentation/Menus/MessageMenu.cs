using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
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
            while (true)
            {
                var options = new[] { "Nova poruka", "Moji razgovori" };
                int choice = KeyboardHelper.MenuGeneratorWithHybridInput(options.Length, "Razgovori", options);

                switch (choice)
                {
                    case 0: 
                        Environment.Exit(0); 
                        break;
                    case 1: 
                        await NewMessageAsync(); 
                        break;
                    case 2: 
                        await MyChatsAsync(); 
                        break;
                    case -1: 
                        return;
                }
            }
        }

        private async Task NewMessageAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Nova poruka");

            var users = (await messageService.GetUsersWithoutConversationAsync(_currentUser.UserId)).ToList();
            if (!users.Any())
            {
                Console.WriteLine("Nema novih korisnika");
                ConsoleHelper.Continue();
                return;
            }

            var selectedUser = MenuHelper.SelectFromList(users, "Dostupni korisnici", u => $"{u.Name} ({u.Role})");
            if (selectedUser == null) 
                return;

            var chatScreen = new ChatScreen(_currentUser, selectedUser.Id, _serviceProvider);
            await chatScreen.ShowAsync();
        }

        private async Task MyChatsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();

            ConsoleHelper.Title("Moji razgovori");

            var partners = (await messageService.GetConversationPartnersAsync(_currentUser.UserId)).ToList();
            if (!partners.Any())
            {
                Console.WriteLine("Nema aktivnih razgovora.");
                ConsoleHelper.Continue();
                return;
            }

            var selectedPartner = MenuHelper.SelectFromList(partners, "Moji razgovori",
                p => p.Name == "[Izbrisan korisnik]" ? p.Name : $"{p.Name} ({p.Role})");
            if (selectedPartner == null) 
                return;

            var chatScreen = new ChatScreen(_currentUser, selectedPartner.Id, _serviceProvider);
            await chatScreen.ShowAsync();
        }
    }
}
