using PassForge.Core.Models;
using PassForge.Console.Services;

namespace PassForge.Console.Menus
{
    public static class Main
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleUI.WriteHeader("=== PassForge Console ===");

                System.Console.WriteLine("1. Generate password list");
                System.Console.WriteLine("2. Configure options");
                System.Console.WriteLine("0. Exit");

                var choice = UserInput.Prompt("Enter your choice");

                switch (choice)
                {
                    case "1":
                        Generate.Show();
                        break;
                    case "2":
                        Options.Show();
                        break;
                    case "0":
                        ConsoleUI.WriteInfo("Goodbye!");
                        return;
                    default:
                        ConsoleUI.WriteError("Invalid choice, please try again.");
                        break;
                }
            }
        }
    }
}
