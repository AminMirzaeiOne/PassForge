using PassForge.Console.Services;

namespace PassForge.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Fast CLI mode if any args are provided
            if (args is { Length: > 0 } && Args.TryHandleFastMode(args, out var exit))
            {
                Environment.Exit(exit);
                return;
            }

            // Interactive menu mode
            PassForge.Console.Menus.Main.Show();
        }
    }
}
