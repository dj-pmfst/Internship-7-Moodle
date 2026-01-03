namespace Moodle.Presentation.Helpers
{
    public static class KeyboardHelper
    {
        public static int MenuGeneratorWithHybridInput(int n, string title, string?[] options)
        {
            int selectedIndex = 0;
            string numberInput = "";
            int maxIndex = n + 1; 

            while (true)
            {
                Console.Clear();
                ConsoleHelper.Title(title);

                for (int i = 0; i < n; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {i + 1} - {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {i + 1} - {options[i]}");
                    }
                }

                if (selectedIndex == n)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (title == "Glavni izbornik")
                    {
                        Console.WriteLine($"> {n + 1} - Odjava");
                    }
                    else if (title != "Moodle")
                    {
                        Console.WriteLine($"> {n + 1} - Povratak");
                    }
                    Console.ResetColor();
                }
                else
                {
                    if (title == "Glavni izbornik")
                    {
                        Console.WriteLine($"  {n + 1} - Odjava");
                    }
                    else if (title != "Moodle")
                    {
                        Console.WriteLine($"  {n + 1} - Povratak");
                    }
                }

                if (selectedIndex == n + 1)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"> 0 - Izlaz iz aplikacije");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  0 - Izlaz iz aplikacije");
                    Console.ResetColor();
                }

                ConsoleHelper.KeyboardNavigation();

                if (!string.IsNullOrEmpty(numberInput))
                {
                    Console.Write($"\nUneseni broj: {numberInput}_");
                }

                var keyInfo = Console.ReadKey(true);
                var key = keyInfo.Key;
                char keyChar = keyInfo.KeyChar;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : maxIndex;
                        numberInput = ""; 
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = selectedIndex < maxIndex ? selectedIndex + 1 : 0;
                        numberInput = ""; 
                        break;

                    case ConsoleKey.Escape:
                        return -1; 

                    case ConsoleKey.Enter:
                        if (!string.IsNullOrEmpty(numberInput))
                        {
                            if (int.TryParse(numberInput, out int typed))
                            {
                                if (typed == 0) return 0; 
                                if (typed >= 1 && typed <= n) return typed; 
                                if (typed == n + 1) return -1; 
                            }
                            numberInput = "";
                        }
                        else
                        {
                            if (selectedIndex < n)
                                return selectedIndex + 1; 
                            else if (selectedIndex == n)
                                return -1; 
                            else
                                return 0; 
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (numberInput.Length > 0)
                        {
                            numberInput = numberInput.Substring(0, numberInput.Length - 1);
                        }
                        break;

                    default:
                        if (char.IsDigit(keyChar))
                        {
                            numberInput += keyChar;

                            if (int.TryParse(numberInput, out int autoSelect))
                            {
                                if (autoSelect >= 0 && autoSelect <= n + 1)
                                {
                                    if (autoSelect == 0)
                                        selectedIndex = n + 1;
                                    else if (autoSelect == n + 1)
                                        selectedIndex = n; 
                                    else if (autoSelect >= 1 && autoSelect <= n)
                                        selectedIndex = autoSelect - 1; 
                                }
                            }
                        }
                         break;
                }
            }
        }
    }
}