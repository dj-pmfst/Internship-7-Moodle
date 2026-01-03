using Moodle.Application.DTOs.Auth;
using Moodle.Application.Services;
using Moodle.Presentation.Helpers;

namespace Moodle.Presentation.Menus
{
    public class AuthenticationMenu
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationMenu(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<LoginResponse?> ShowAsync()
        {
            while (true)
            {
                var options = new[] { "Prijava", "Registracija" };
                int choice = KeyboardHelper.MenuGeneratorWithHybridInput(options.Length, "Moodle", options);

                switch (choice)
                {
                    case 0: 
                        return null;
                    case 1:
                        var loginResult = await LoginAsync();
                        if (loginResult != null) return loginResult;
                        break;
                    case 2:
                        await RegisterAsync();
                        break;
                }
            }
        }

        private async Task<LoginResponse?> LoginAsync()
        {
            ConsoleHelper.Title("Prijava");
            var email = InputHelper.ReadEmail("Email: ");
            var password = InputHelper.ReadPassword("Šifra: ");

            var result = await _authenticationService.LoginAsync(new LoginRequest { Email = email, Password = password });

            if (ConsoleHelper.DisplayResult(result, "Uspješna prijava."))
            {
                ConsoleHelper.Continue();
                return result.Value;
            }

            Console.WriteLine("Pričekajte 30s prije ponovnog pokušaja...");
            await Task.Delay(30000);
            return null;
        }

        private async Task RegisterAsync()
        {
            ConsoleHelper.Title("Registracija");

            var name = InputHelper.StringValid("Ime: ");
            var email = InputHelper.ReadEmail("Email: ");
            var password = InputHelper.ReadPassword("Šifra: ");
            var confirmPassword = InputHelper.ErrInput("Ponovno unesite šifru: ", password);

            var captcha = _authenticationService.GenerateCaptcha();
            Console.WriteLine($"\nCaptcha: {captcha}");
            string captchaInput;

            do
            {
                captchaInput = InputHelper.StringValid("Unos: ");
                if (captchaInput != captcha)
                {
                    Console.WriteLine("Neispravan unos. Pokušajte ponovno.");
                }
            } while (captchaInput != captcha);

            var request = new RegisterRequest
            {
                Name = name,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                Captcha = captchaInput
            };

            ConsoleHelper.DisplayResult(await _authenticationService.RegisterAsync(request), "Uspješna registracija.");
            ConsoleHelper.Continue();
        }
    }
}
