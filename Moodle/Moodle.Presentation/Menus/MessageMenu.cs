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
            var options = new List<string> { "Nova poruka", "Moji razgovori" };

            int n = options.Count;

            MenuHelper.MenuGenerator(n, "Razgovori", options.ToArray());

            var choice = MenuHelper.GetMenuChoice(n);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    await NewMessageAsync();
                    break;
                case 2:
                    await MyChatsAsync();
                    break;
            }

            ConsoleHelper.Continue();
        }

        private async Task NewMessageAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();

            ConsoleHelper.Title("Nova poruka");

            var users = await messageService.GetUsersWithoutConversationAsync(_currentUser.UserId);
            var userList = users.ToList();

            if (!userList.Any())
            {
                Console.WriteLine("Nema novih korisnika");
                ConsoleHelper.Continue();
                return;
            }

            Console.WriteLine("Available users:");
            for (int i = 0; i < userList.Count; i++)
            {
                Console.WriteLine($"{userList[i].Id}. {userList[i].Name} ({userList[i].Role})");
            }

            Console.WriteLine("\n Unesite ID korisnika kojem želite poslati poruku: ");
            var userId = InputHelper.ReadInt(1, userList.Count());

            var chatScreen = new ChatScreen(_currentUser, userId, _serviceProvider);
            await chatScreen.ShowAsync();
        }

        private async Task MyChatsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();

            ConsoleHelper.Title("Moji razgovori");

            var partners = await messageService.GetConversationPartnersAsync(_currentUser.UserId);
            var partnerList = partners.ToList();

            if (!partnerList.Any())
            {
                Console.WriteLine("Nema razgovora");
                ConsoleHelper.Continue();
                return;
            }

            for (int i = 0; i < partnerList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {partnerList[i].Name} ({partnerList[i].Role})");
            }

            Console.WriteLine("\n Unesite ID razgovora: ");
            var choice = InputHelper.ReadInt(1, partnerList.Count());
            var selectedUser = partnerList[choice - 1];

            var chatScreen = new ChatScreen(_currentUser, selectedUser.Id, _serviceProvider);
            await chatScreen.ShowAsync();
        }
    }
}
