namespace PassForge.Console.Services
{
    public static class ConsoleUI
    {
        public static void WriteHeader(string text)
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine(text);
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        public static void WriteError(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("ERROR: " + message);
            System.Console.ResetColor();
        }

        public static void WriteInfo(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }
    }
}
