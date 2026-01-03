namespace Moodle.Presentation.Helpers
{
    public static class InputHelper
    {
        public static bool Confirmation(int id, string type)
        {
            Console.Write($"\nJeste li sigurni da želite {type} {id}? (y/n): ");
            var message = Console.ReadLine()?.ToLower();
            return message == "y" || message == "yes" || message == "da";
        }

        public static int ReadInt(int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result) && result >= min && result <= max)
                    return result;

                Console.Write($"\nNeispravan unos. Broj između {min} i {max}: ");
            }
        }

        public static int ReadChoice(string prompt, int maxChoice)
        {
            Console.Write(prompt);
            return ReadInt(1, maxChoice);
        }

        public static string ReadPassword(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                var password = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.Write("\nŠifra ne može biti prazna: ");
                    continue;
                }

                if (password.Length < 4)
                {
                    Console.Write("\nŠifra mora biti duža od 4 znaka: ");
                    continue;
                }

                if (password.Contains(" "))
                {
                    Console.Write("\nŠifra ne može sadržavati razmake: ");
                    continue;
                }

                return password;
            }
        }

        public static string ReadEmail(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                var input = Console.ReadLine()?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("\nEmail ne može biti prazan: ");
                    continue;
                }

                if (!input.Contains("@") || !input.Contains("."))
                {
                    Console.Write("\nNeispravan email format: ");
                    continue;
                }

                return input;
            }
        }

        public static string StringValid(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                var input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input))
                    return input;

                Console.Write("\nUnos ne može biti prazan: ");
            }
        }

        public static DateTime DateValid(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out DateTime date) && date.Year <= 2027)
                    return date;

                Console.Write("\nNeispravan datum (dd/MM/yyyy): ");
            }
        }
    }
}