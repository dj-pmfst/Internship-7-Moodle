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

        public static string ErrInput()
        {
            Console.Write("\nNeispravan unos. \nUnesite opet. ");
            return Console.ReadLine();
        }

        public static void KeyboardNavigation()
        {
            Console.WriteLine("\n-----------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("↑/↓ + Enter  ili  unesi broj + Enter  |  ESC za povratak");
            Console.ResetColor();
        }
    }
}
