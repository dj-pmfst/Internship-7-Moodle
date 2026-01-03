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
                var options = new[] { "Brisanje korisnika", "Uređivanje emaila", "Promjena uloge" };
                int choice = KeyboardHelper.MenuGeneratorWithHybridInput(options.Length, "Upravljanje korisnicima", options);

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
                    case -1: 
                        return;
                }
            }
        }

        private async Task<List<UserDTO>> GetAllUsersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<UserService>();
            var students = await userService.GetAllStudentsAsync();
            var professors = await userService.GetAllProfessorsAsync();

            return students.Concat(professors).ToList();
        }

        private async Task DeleteUserAsync()
        {
            ConsoleHelper.Title("Brisanje korisnika");

            var allUsers = await GetAllUsersAsync();
            var user = MenuHelper.SelectFromList(allUsers, "Odaberite korisnika za brisanje", u => $"{u.Name} - {u.Email}");
            if (user == null)
                return;

            if (!InputHelper.Confirmation(user.Id, "brisanje")) 
                return;

            using var scope = _serviceProvider.CreateScope();
            var result = await scope.ServiceProvider.GetRequiredService<UserService>().DeleteUserAsync(user.Id);

            ConsoleHelper.DisplayResult(result, $"Uspješno izbrisan korisnik ID - {user.Id}");
            ConsoleHelper.Continue();
        }

        private async Task EditEmailAsync()
        {
            ConsoleHelper.Title("Izmjena emaila");

            var allUsers = await GetAllUsersAsync();
            var user = MenuHelper.SelectFromList(allUsers, "Odaberite korisnika", u => $"{u.Name} - {u.Email}");
            if (user == null) 
                return;

            var newEmail = InputHelper.ReadEmail($"Trenutni email: {user.Email}\nNovi email: ");
            if (!InputHelper.Confirmation(user.Id, "izmjeniti")) 
                return;

            using var scope = _serviceProvider.CreateScope();
            var result = await scope.ServiceProvider.GetRequiredService<UserService>().UpdateEmailAsync(
                new UpdateUserEmailRequest { UserId = user.Id, NewEmail = newEmail });

            ConsoleHelper.DisplayResult(result, $"Email uspješno ažuriran na: {newEmail}");
            ConsoleHelper.Continue();
        }

        private async Task RoleChangeAsync()
        {
            ConsoleHelper.Title("Izmjena uloge");

            var allUsers = await GetAllUsersAsync();
            var user = MenuHelper.SelectFromList(allUsers, "Odaberite korisnika", u => $"{u.Name} - {u.Email} ({u.Role})");
            if (user == null) 
                return;

            Console.WriteLine($"Trenutna uloga: {user.Role}");
            Console.WriteLine($"Nova uloga: {(user.Role == Domain.Enums.Roles.student ? "profesor" : "student")}");
            if (!InputHelper.Confirmation(user.Id, "izmjenu uloge"))
                return;

            using var scope = _serviceProvider.CreateScope();
            var result = await scope.ServiceProvider.GetRequiredService<UserService>().ChangeRoleAsync(
                new ChangeRoleRequest { UserId = user.Id });

            ConsoleHelper.DisplayResult(result, $"Uspješno izmjenjena uloga korisnika {user.Name}");
            ConsoleHelper.Continue();
        }
    }
}
