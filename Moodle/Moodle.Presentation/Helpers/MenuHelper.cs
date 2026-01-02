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
            if (title != "Glavni izbornik" || title != "Moodle")
            {
                Console.WriteLine($"{n+1} - Povratak na prethodni izbornik", n+1);
            }
            else if (title == "Moodle")
            {
                Console.WriteLine($"{0} - Odjava", n+1);
            }
            Console.WriteLine("0 - Izlazak iz aplikacije");
            Console.WriteLine();
        }

        public static int GetMenuChoice(int n)
        {
            return InputHelper.ReadInt(0, n);
        }
    }
}
