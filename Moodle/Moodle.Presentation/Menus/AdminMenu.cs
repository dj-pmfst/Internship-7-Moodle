using Microsoft.Extensions.DependencyInjection;
using Moodle.Application.DTOs.Auth;
using Moodle.Application.DTOs.User;
using Moodle.Application.Services;
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

            Console.Write("\nOdabir: ");
            var choice = MenuHelper.GetMenuChoice(n);

            switch (choice)
            {
                case 0:
                    return;
                case 1:
                    await DeleteUserAsync();
                    break;
                case 2:
                    await EditEmailAsync();
                    break;
                case 3:
                    await RoleChangeAsync();
                    break;
            }

            ConsoleHelper.Continue();
        }

        private async Task DeleteUserAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Brisanje korisnika");

            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();
            var allUsers = students.Concat(professors).ToList();

            if (!allUsers.Any())
            {
                Console.WriteLine("Nema korisnika");
                return;
            }

            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email}");
            }              

            int userId;
            var validIds = allUsers.Select(u => u.Id).ToHashSet();

            Console.Write("\nUnesite ID korisnika kojeg želite izbrisati: ");
            while (true)
            {               
                userId = InputHelper.ReadInt(); 
                if (validIds.Contains(userId))
                    break;

                ConsoleHelper.ErrInput();
            }

            if (InputHelper.Confirmation(userId, "brisanje"))
            {
                var result = await userService.DeleteUserAsync(userId);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"Uspješno izbirsan korisnik ID - {userId}");
                }
                else
                {
                    result.ValidationResult.GetErrorMessages().FirstOrDefault();
                }
            }
        }

        private async Task EditEmailAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Izmjena emiala");

            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();
            var allUsers = students.Concat(professors).ToList();

            if (!allUsers.Any())
            {
                Console.WriteLine("Nema korisnika za uređivanje.");
                return;
            }

            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email}");
            }

            int userId;
            var validIds = allUsers.Select(u => u.Id).ToHashSet();

            Console.Write("\nUnesite ID korisnika kojeg želite urediti: ");

            while (true)
            {
                userId = InputHelper.ReadInt();
                if (validIds.Contains(userId))
                    break;

                ConsoleHelper.ErrInput();
            }

            var newEmail = InputHelper.ReadEmail("Unesite novi email: ");

            var request = new UpdateUserEmailRequest
            {
                UserId = userId,
                NewEmail = newEmail
            };

            if (InputHelper.Confirmation(userId, "uređivanje"))
            {
                var result = await userService.UpdateEmailAsync(request);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"Uspješno izmjenjen email korisnika ID - {userId}");
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

        private async Task RoleChangeAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Izmjena uloge");

            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();
            var allUsers = students.Concat(professors).ToList();

            if (!allUsers.Any())
            {
                Console.WriteLine("Nema korisnika");
                return;
            }

            foreach (var user in allUsers)
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email} ({user.Role})");

            int userId;
            var validIds = allUsers.Select(u => u.Id).ToHashSet();
            while (true)
            {
                Console.Write("\nUnesite ID korisnika kojem želite izmjeniti ulogu: ");
                userId = InputHelper.ReadInt();
                if (validIds.Contains(userId))
                    break;

                ConsoleHelper.ErrInput();
            }

            if (InputHelper.Confirmation(userId, "uređivanje"))
            {
                var request = new ChangeRoleRequest { UserId = userId };
                var result = await userService.ChangeRoleAsync(request);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"Uspješno izmjenjena uloga korisnika ID - {userId}");
                }
                else
                {
                   result.ValidationResult.GetErrorMessages().FirstOrDefault();
                }
            }
        }
    }
}
