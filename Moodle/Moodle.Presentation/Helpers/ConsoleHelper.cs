namespace Moodle.Presentation.Helpers
{
    public static class ConsoleHelper
    {
        public static void Clear()
        {
            Console.Clear(); 
        }

        public static void Title(string title)
        {
            Clear();
            Console.WriteLine("\n");
            Console.WriteLine("{0}", title);
            Console.WriteLine("\n\n");
        }

        public static void Continue()
        {
            Console.WriteLine("\nPritisnite bilo koju tipku za nastavak...");
            Console.ReadKey();
        }

        public static void KeyboardNavigation()
        {
            Console.WriteLine("\n-----------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("↑/↓ + Enter  ili  unesi broj + Enter  |  ESC za povratak");
            Console.ResetColor();
        }

        public static bool DisplayResult(dynamic result, string successMessage)
        {
            if (result.IsSuccess)
            {
                Console.WriteLine(successMessage);
                return true;
            }
            else
            {
                foreach (var error in result.ValidationResult.GetErrorMessages())
                    Console.WriteLine($"  - {error}");
                return false;
            }
        }
    }
}
