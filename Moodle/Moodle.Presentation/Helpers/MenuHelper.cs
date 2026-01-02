namespace Moodle.Presentation.Helpers
{
    public class MenuHelper
    {
        public static void MenuGenerator(int n, string title, string[] text)
        {
            ConsoleHelper.Title(title);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i+1} - {text[i]}");
            }
            if (title != "Glavni izbornik" && title != "Moodle")
            {
                Console.WriteLine($"{n+1} - Povratak na prethodni izbornik");
            }
            if (title == "Glavni izbornik")
            {
                Console.WriteLine($"{n+1} - Odjava");
            }

            Console.WriteLine("0 - Izlazak iz aplikacije");       
            Console.WriteLine();
        }

        public static int GetMenuChoice(int n)
        {
            Console.WriteLine($"n: {n}");
            Console.Write("\nOdabir: ");
            return InputHelper.ReadInt(0, n);
        }
    }
}
