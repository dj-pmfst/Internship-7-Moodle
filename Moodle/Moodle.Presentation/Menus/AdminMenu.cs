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
            while (true)
            {
                var options = new List<string> { "Brisanje korisnika", "Uređivanje emaila", "Promjena uloge" };

                int n = options.Count;

                MenuHelper.MenuGenerator(n, "Upravljanje korisnicima", options.ToArray());

                var choice = MenuHelper.GetMenuChoice(4);

                switch (choice)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        await DeleteUserAsync();
                        break;
                    case 2:
                        await EditEmailAsync();
                        break;
                    case 3:
                        await RoleChangeAsync();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private async Task DeleteUserAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Brisanje korisnika");

            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();
            var allUsers = students.Concat(professors).ToList();

            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email}");
            }

            var validIds = allUsers.Select(u => u.Id).ToList();

            int userId;
            while (true)
            {
                Console.Write("\nUnesite ID korisnika kojeg želite izbrisati: ");
                userId = InputHelper.ReadInt(); 

                if (validIds.Contains(userId))
                    break;

                Console.WriteLine($"ID {userId} ne postoji. Pokušajte ponovno.");
            }

            if (!InputHelper.Confirmation(userId, "brisanje"))
            {
                return;
            }

            var result = await userService.DeleteUserAsync(userId);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Uspješno izbrisan korisnik ID - {userId}");
            }
            else
            {
                result.ValidationResult.GetErrorMessages().FirstOrDefault();
            }

            ConsoleHelper.Continue();
        }

        private async Task EditEmailAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();

            ConsoleHelper.Title("Izmjena emaila");

            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();
            var allUsers = students.Concat(professors).ToList();

            if (!allUsers.Any())
            {
                Console.WriteLine("Nema korisnika.");
                ConsoleHelper.Continue();
                return;
            }

            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email}");
            }

            var validIds = allUsers.Select(u => u.Id).ToList();

            int userId;
            while (true)
            {
                Console.Write("\nUnesite ID korisnika: ");
                userId = InputHelper.ReadInt();

                if (validIds.Contains(userId))
                    break;

                Console.WriteLine($"ID {userId} ne postoji. Pokušajte ponovno.");
            }

            var selectedUser = allUsers.First(u => u.Id == userId);
            Console.WriteLine($"\nTrenutni email: {selectedUser.Email}");

            var newEmail = InputHelper.ReadEmail("Unesite novi email: ");

            Console.WriteLine($"\nStari email: {selectedUser.Email}");
            Console.WriteLine($"Novi email:  {newEmail}");

            if (!InputHelper.Confirmation(userId, "izmjeniti"))
            {
                Console.WriteLine("otkazano");
                ConsoleHelper.Continue();
                return;
            }

            var request = new UpdateUserEmailRequest
            {
                UserId = userId,
                NewEmail = newEmail
            };

            var result = await userService.UpdateEmailAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Email uspješno ažuriran na: {result.Value!.Email}");
            }
            else
            {
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine($"  - {error}");
                }
            }

            ConsoleHelper.Continue();
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
                Console.WriteLine("Nema korisnika.");
                ConsoleHelper.Continue();
                return;
            }

            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.Id} - {user.Name} - {user.Email} ({user.Role})");
            }

            var validIds = allUsers.Select(u => u.Id).ToHashSet();

            int userId;
            while (true)
            {
                Console.Write("\nUnesite ID korisnika: ");  
                userId = InputHelper.ReadInt();

                if (validIds.Contains(userId))
                    break;

                Console.WriteLine($"ID {userId} ne postoji. Pokušajte ponovno."); 
            }

            var selectedUser = allUsers.First(u => u.Id == userId);
            Console.WriteLine($"\nTrenutna uloga: {selectedUser.Role}");
            Console.WriteLine($"Nova uloga bit će: {(selectedUser.Role == Domain.Enums.Roles.student ? "profesor" : "student")}");

            if (!InputHelper.Confirmation(userId, "izmjenu uloge"))
            {
                Console.WriteLine("Otkazano.");
                ConsoleHelper.Continue();
                return;
            }

            var request = new ChangeRoleRequest { UserId = userId };
            var result = await userService.ChangeRoleAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Uspješno izmjenjena uloga korisnika {selectedUser.Name}");
                Console.WriteLine($"Nova uloga: {result.Value!.Role}");
            }
            else
            {
                Console.WriteLine("Neuspješna izmjena:");
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine($"  - {error}");
                }
            }

            ConsoleHelper.Continue();
        }
    }
}
