using PassForge.Console.Services;
using PassForge.Core.Models;

namespace PassForge.Console.Menus
{
    public static class Options
    {
        private static PasswordOptions _options = new PasswordOptions();

        public static PasswordOptions GetOptions() => _options;

        public static void Show()
        {
            while (true)
            {
                ConsoleUI.WriteHeader("=== Password Options ===");

                System.Console.WriteLine($"1. Min Length: {_options.MinLength}");
                System.Console.WriteLine($"2. Max Length: {_options.MaxLength}");
                System.Console.WriteLine($"3. Include Lowercase: {_options.IncludeLowercase}");
                System.Console.WriteLine($"4. Include Uppercase: {_options.IncludeUppercase}");
                System.Console.WriteLine($"5. Include Digits: {_options.IncludeDigits}");
                System.Console.WriteLine($"6. Include Special: {_options.IncludeSpecial}");
                System.Console.WriteLine($"7. Special Separators: {string.Join("", _options.Separators)}");
                System.Console.WriteLine($"8. Use Leet Transformations: {_options.UseLeet}");
                System.Console.WriteLine($"9. Include Reversed Words: {_options.IncludeReversed}");
                System.Console.WriteLine($"10. Max Count: {_options.MaxCount}");

                System.Console.WriteLine("0. Back to main menu");

                var choice = UserInput.Prompt("Choose an option to edit");
                if (choice == "0") return;

                switch (choice)
                {
                    case "1":
                        _options.MinLength = UserInput.PromptInt("Enter min length", _options.MinLength);
                        break;
                    case "2":
                        _options.MaxLength = UserInput.PromptInt("Enter max length", _options.MaxLength);
                        break;
                    case "3":
                        _options.IncludeLowercase = UserInput.PromptBool("Include lowercase?");
                        break;
                    case "4":
                        _options.IncludeUppercase = UserInput.PromptBool("Include uppercase?");
                        break;
                    case "5":
                        _options.IncludeDigits = UserInput.PromptBool("Include digits?");
                        break;
                    case "6":
                        _options.IncludeSpecial = UserInput.PromptBool("Include special characters?");
                        break;
                    case "7":
                        var separators = UserInput.Prompt("Enter allowed separators (e.g. ._-)", string.Join("", _options.Separators));
                        _options.Separators = separators.Distinct().ToList();
                        break;
                    case "8":
                        _options.UseLeet = UserInput.PromptBool("Enable leet transformations?");
                        break;
                    case "9":
                        _options.IncludeReversed = UserInput.PromptBool("Include reversed variants?");
                        break;
                    case "10":
                        _options.MaxCount = UserInput.PromptInt("Enter max password count", _options.MaxCount);
                        break;
                    default:
                        ConsoleUI.WriteError("Invalid choice");
                        break;
                }
            }
        }
    }
}
