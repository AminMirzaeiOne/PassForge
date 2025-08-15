using System;

namespace PassForge.Console.Services
{
    public static class ConsoleUI
    {
        public static void WriteTitle(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("=== " + text + " ===");
            System.Console.ResetColor();
        }

        public static void WriteOption(int index, string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.Write($"[{index}] ");
            System.Console.ResetColor();
            System.Console.WriteLine(text);
        }

        public static void WriteHint(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
        }

        public static void WriteError(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("خطا: " + text);
            System.Console.ResetColor();
        }

        public static void WriteSuccess(string text)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
        }

        public static void Separator() => System.Console.WriteLine(new string('-', 50));
    }
}
