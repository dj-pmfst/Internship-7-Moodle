using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
using Moodle.Moodle.Application.DTOs.Message;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class ChatScreen
    {
        private readonly LoginResponse _currentUser;
        private readonly int _recieverId;
        private readonly IServiceProvider _serviceProvider;

        public ChatScreen(LoginResponse currentUser, int receiverId, IServiceProvider serviceProvider)
        {
            _currentUser = currentUser;
            _recieverId = receiverId;
            _serviceProvider = serviceProvider;
        }

        public async Task ShowAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<MessageService>();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            var reciever = await userService.GetByIdAsync(_recieverId);

            if (reciever == null)
            {
                Console.WriteLine("Korisnik nije pronađen");
                ConsoleHelper.Continue();
                return;
            }

            while (true)
            {
                ConsoleHelper.Title($"Razgovor s {reciever.Name}");

                var messages = await messageService.GetConversationAsync(
                    _currentUser.UserId,
                    _recieverId,
                    _currentUser.UserId);

                var messageList = messages.ToList();

                if (!messageList.Any())
                {
                    Console.WriteLine("Započni razgovor");
                }
                else
                {
                    foreach (var message in messageList)
                    {
                        if (message.IsSentByCurrentUser)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"{message.FormattedDate} [You]");
                            Console.WriteLine($"  {message.Text}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[{message.SenderName}] {message.FormattedDate}");
                            Console.WriteLine($"  {message.Text}");
                            Console.ResetColor();
                        }
                        Console.WriteLine();
                    }
                }

                var options = new List<string> { "Pošalji poruku", "Osvježi", "Nazad" };

                MenuHelper.MenuGenerator(3, "\n", options.ToArray());

                var choice = InputHelper.ReadInt(0,options.Count());

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

        private async Task SendMessageAsync(MessageService messageService)
        {
            Console.WriteLine();
            var text = InputHelper.StringValid("Unesi poruku: ");

            var request = new SendMessageRequest
            {
                SenderId = _currentUser.UserId,
                ReceiverId = _recieverId,
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
                ConsoleHelper.Continue();
            }
        }
    }
}