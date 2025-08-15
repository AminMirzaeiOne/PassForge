using PassForge.Console.Services;

namespace PassForge.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (args is { Length: > 0 })
            {
                if (PassForge.Console.Services.Args.TryHandleFastMode(args, out var exitCode))
                {
                    Environment.Exit(exitCode);
                    return;
                }
            }


            PassForge.Console.Menus.Main.Show();
        }
    }
}
