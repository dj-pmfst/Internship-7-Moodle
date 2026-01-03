using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.DTOs.Message;
using Moodle.Application.Services;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class ChatScreen
    {
        private readonly LoginResponse _currentUser;
        private readonly int _receiverId;
        private readonly IServiceProvider _serviceProvider;

        public ChatScreen(LoginResponse currentUser, int receiverId, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _receiverId = receiverId;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var receiver = await userService.GetByIdAsync(_receiverId);

            if (receiver == null)
            {
                await ShowReadOnlyConversationAsync(messageService);
                return;
            }

            while (true)
            {
                Console.Clear();
                ConsoleHelper.Title($"Razgovor s {receiver.Name}");

                var messages = await messageService.GetConversationAsync(
                    _currentUser.UserId,
                    _receiverId);

                var messageList = messages.ToList();

                foreach (var message in messageList)
                {
                    if (message.IsSentByCurrentUser)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{message.FormattedDate} [Vi]");
                        Console.WriteLine($"  {message.Text}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"{receiver.Name} {message.FormattedDate}");
                        Console.WriteLine($"  {message.Text}");
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }

                if (!messageList.Any())
                {
                    Console.WriteLine("Započni razgovor");
                }

                var options = new List<string> { "Pošalji poruku", "Osvježi", "Povratak"};

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"{i + 1} - {options[i]}");
                }

                var choice = MenuHelper.GetMenuChoice(options.Count());

                switch (choice)
                {
                    case 1:
                        await SendMessageAsync(messageService);
                        break;
                    case 2:
                        break;
                    case 3:
                        return;
                }
            }
        }

        private async Task ShowReadOnlyConversationAsync(MessageService messageService)
        {
            Console.Clear();
            ConsoleHelper.Title("Razgovor s [Izbrisan korisnik]");

            var messages = await messageService.GetConversationAsync(
                _currentUser.UserId,
                _receiverId
            );

            var messageList = messages.ToList();

            if (!messageList.Any())
            {
                Console.WriteLine("Nema poruka.");
            }
            else
            {
                foreach (var message in messageList)
                {
                    if (message.IsSentByCurrentUser)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{message.FormattedDate} [Vi]");
                        Console.WriteLine($"  {message.Text}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"[Izbrisan korisnik] {message.FormattedDate}");
                        Console.WriteLine($"  {message.Text}"); 
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

            Console.WriteLine("\nOvaj korisnik je izbrisan. Ne možete poslati nove poruke.");
            ConsoleHelper.Continue();
        }

        private async Task SendMessageAsync(MessageService messageService)
        {
            var text = InputHelper.StringValid("Unesi poruku: ");

            var request = new SendMessageRequest
            {
                SenderId = _currentUser.UserId,
                ReceiverId = _receiverId,
                Text = text
            };

            var result = await messageService.SendMessageAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine("Uspješno poslano");
                await Task.Delay(500);
            }
            else
            {
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}