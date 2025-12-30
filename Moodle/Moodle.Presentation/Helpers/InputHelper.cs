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

        public static int ReadInt(int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    return result;

                ConsoleHelper.ErrInput();
            }
        }

        public static T ReadValid<T>(string prompt, Func<string, (bool isValid, T? value, string? error)> validator)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim() ?? string.Empty;

                var (isValid, value, error) = validator(input);

                if (isValid && value != null)
                    return value;

                ConsoleHelper.ErrInput();
            }
        }

        public static string ReadPassword(string password)
        {
            while (string.IsNullOrWhiteSpace(password) || password.Length < 4 || password.Contains(" "))
            {
                Console.WriteLine("Šifra ne može biti prazna, ni kraća od 4 znaka.");
                Console.Write("Unesite šifru: ");
                password = Console.ReadLine();
            }
            return password;
        }

        public static DateTime DateValid(string dateInput)
        {
            while (!DateTime.TryParse(dateInput, out DateTime date) || date.Year > 2026)
            {
                dateInput = ConsoleHelper.ErrInput(); ;
            }
            return DateTime.Parse(dateInput);
        }

        public static string ReadEmail(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.ErrInput();
                }

                if (!input.Contains("@") || !input.Contains("."))
                {
                    ConsoleHelper.ErrInput();
                }

                return input;
            }
        }

        public static string StringValid(string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.ErrInput();
                }

                return input;
            }
        }
    }
}
