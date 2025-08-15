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
                System.Console.WriteLine($"7. Special Characters: {_options.SpecialCharacters}");
                System.Console.WriteLine($"8. Use Leet Transformations: {_options.UseLeet}");
                System.Console.WriteLine($"9. Include Reversed Words: {_options.IncludeReversed}");
                System.Console.WriteLine($"10. Word Case Variants: {_options.IncludeWordCaseVariants}");
                System.Console.WriteLine($"11. Require At Least One From Each Category: {_options.RequireAtLeastOneFromEachSelectedCategory}");
                System.Console.WriteLine($"12. Separators: \"{_options.Separators}\"");
                System.Console.WriteLine($"13. Max Tokens Per Password: {_options.MaxTokensPerPassword}");
                System.Console.WriteLine($"14. Append Common Numbers: {_options.AppendCommonNumbers}");
                System.Console.WriteLine($"15. Custom Suffixes: {string.Join(", ", _options.CustomSuffixes)}");
                System.Console.WriteLine($"16. Max Count: {_options.MaxCount}");

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
                        _options.SpecialCharacters = UserInput.Prompt("Enter allowed special characters", _options.SpecialCharacters);
                        break;
                    case "8":
                        _options.UseLeet = UserInput.PromptBool("Enable leet transformations?");
                        break;
                    case "9":
                        _options.IncludeReversed = UserInput.PromptBool("Include reversed variants?");
                        break;
                    case "10":
                        _options.IncludeWordCaseVariants = UserInput.PromptBool("Include case variants?");
                        break;
                    case "11":
                        _options.RequireAtLeastOneFromEachSelectedCategory = UserInput.PromptBool("Require at least one from each selected category?");
                        break;
                    case "12":
                        _options.Separators = UserInput.Prompt("Enter separators as string (e.g. ._-)", _options.Separators);
                        break;
                    case "13":
                        _options.MaxTokensPerPassword = UserInput.PromptInt("Enter max tokens per password", _options.MaxTokensPerPassword);
                        break;
                    case "14":
                        _options.AppendCommonNumbers = UserInput.PromptBool("Append common numbers?");
                        break;
                    case "15":
                        var input = UserInput.Prompt("Enter custom suffixes separated by commas", string.Join(",", _options.CustomSuffixes));
                        _options.CustomSuffixes = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                        break;
                    case "16":
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
