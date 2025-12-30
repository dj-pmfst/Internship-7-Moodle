namespace Moodle.Presentation.Helpers
{
    public static class InputHelper
    {
        public static bool Confirmation(int id, string type)
        {
            Console.Write("\nJeste li sigurni da želite izmjeniti {0}? (y/n): ", id);
            var message = Console.ReadLine();
            if (message.ToLower() == "y" || message.ToLower() == "yes" || message.ToLower() == "da")
            {
                return true;
            }
            else
            {
                Console.WriteLine("Otkazano {0}", type);
                ConsoleHelper.Continue();
                return false;
            }
        }
        public static string ErrInput()
        {
            Console.Write("\nNeispravan unos. \nUnesite opet:");
            return Console.ReadLine();
        }

        public static T ReadValidated<T>(string prompt, Func<string, (bool isValid, T? value, string? error)> validator)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                var (isValid, value, error) = validator(input);

                if (isValid && value != null)
                    return value;

                ErrInput();
            }
        }

        public static string ReadPassword(string password)
        {
            while (string.IsNullOrWhiteSpace(password) || password.Length < 4 || password.Contains(" "))
            {
                Console.WriteLine("Šifra ne može biti prazna ni kraća od 4 znaka.");
                Console.Write("Unesite šifru: ");
                password = Console.ReadLine();
            }
            return password;
        }

        public static DateTime DateValid(string dateInput)
        {
            while (!DateTime.TryParse(dateInput, out DateTime date) || date.Year > 2026)
            {
                dateInput = ErrInput();
            }
            return DateTime.Parse(dateInput);
        }
    }
}
