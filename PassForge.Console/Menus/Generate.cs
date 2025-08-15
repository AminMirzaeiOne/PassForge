using PassForge.Console.Services;
using PassForge.Console.Views;
using PassForge.Core.Models;
using PassForge.Core.Services;

namespace PassForge.Console.Menus
{
    public static class Generate
    {
        public static void Show()
        {
            ConsoleUI.WriteHeader("=== Generate Password List ===");

            var user = new UserInfo
            {
                FirstName = UserInput.Prompt("First Name"),
                LastName = UserInput.Prompt("Last Name"),
                Nickname = UserInput.Prompt("Nickname (optional)", allowEmpty: true),
                Email = UserInput.Prompt("Email (optional)", allowEmpty: true),
                Phone = UserInput.Prompt("Phone (optional)", allowEmpty: true),
            };

            var dobInput = UserInput.Prompt("Date of Birth (yyyy-MM-dd, optional)", allowEmpty: true);
            if (DateTime.TryParse(dobInput, out var dob)) user.DateOfBirth = dob;

            var ageInput = UserInput.Prompt("Age in years (optional)", allowEmpty: true);
            if (int.TryParse(ageInput, out var age)) user.AgeYears = age;

            var extras = UserInput.Prompt("Extra words (comma separated, optional)", allowEmpty: true);
            if (!string.IsNullOrWhiteSpace(extras))
                user.ExtraWords = extras.Split(',').Select(s => s.Trim()).ToList();

            var generator = new PasswordGenerator(Options.GetOptions());
            var list = generator.Generate(user).ToList();

            PasswordPreview.Show(list, previewCount: 20);

            if (UserInput.PromptBool("Do you want to save the password list to a file?"))
            {
                var path = UserInput.Prompt("Enter file path", "passwords.txt");
                FileExport.ExportToFile(list, path);
            }

            ConsoleUI.WriteInfo("Generation complete. Press any key to return to menu...");
            System.Console.ReadKey();
        }
    }
}
