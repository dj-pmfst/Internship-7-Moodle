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
                var options = new List<string> { "Prijava", "Registracija" };
                MenuHelper.MenuGenerator(2, "Moodle", options.ToArray());

                var choice = MenuHelper.GetMenuChoice(2);

                switch (choice)
                {
                    case 0:
                        return null;
                    case 1:
                        var LoginResult = await LoginAsync();
                        if (LoginResult != null)
                        {
                            return LoginResult;
                        }
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

            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var result = await _authenticationService.LoginAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine($"Uspješna prijava.");
                ConsoleHelper.Continue();
                return result.Value;
            }
            else
            {
                Console.WriteLine("Neuspješna prijava.");
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine(error);
                }

                Console.WriteLine("Pričekajte 30s prije ponovnog pokušaja...");
                await Task.Delay(30000); 
                return null;
            }
        }

        private async Task RegisterAsync()
        {
            ConsoleHelper.Title("Registracija");

            var name = InputHelper.StringValid("Ime: ");
            var email = InputHelper.ReadEmail("Email: ");
            var password = InputHelper.ReadPassword("Šifra: ");
            string confirmPassword;
            while (true)
            {
                confirmPassword = InputHelper.ReadPassword("Ponovno unesite šifru: ");
                if (confirmPassword != password)
                {
                    ConsoleHelper.ErrInput();
                    continue;
                }
                break;
            }

            var captcha = _authenticationService.GenerateCaptcha();
            Console.WriteLine($"\nCaptcha: {captcha}");
            string captchaInput;

            while (true)
            {
                captchaInput = InputHelper.StringValid("Unos: ");

                if (captcha != captchaInput)
                {
                    ConsoleHelper.ErrInput();
                    continue;
                }

                break;
            }


            var request = new RegisterRequest
            {
                Name = name,
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword,
                Captcha = captchaInput
            };

            var result = await _authenticationService.RegisterAsync(request);

            if (result.IsSuccess)
            {
                Console.WriteLine("Uspješna registracija."); ;
            }
            else
            {
                Console.WriteLine("Neuspješna registracija.");
                foreach (var error in result.ValidationResult.GetErrorMessages())
                {
                    Console.WriteLine(error); ;
                }
            }

            ConsoleHelper.Continue();
        }
    }
}
