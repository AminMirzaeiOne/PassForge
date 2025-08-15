namespace PassForge.Console.Services
{
    public static class UserInput
    {
        public static string Prompt(string label, string? defaultValue = null, bool allowEmpty = false)
        {
            System.Console.Write(label + (defaultValue != null ? $" [{defaultValue}]" : "") + ": ");
            var input = System.Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                if (allowEmpty) return string.Empty;
                return defaultValue ?? string.Empty;
            }
            return input;
        }

        public static bool PromptBool(string label, bool defaultValue = true)
        {
            System.Console.Write($"{label} (y/n) [{(defaultValue ? "y" : "n")}]: ");
            var input = System.Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(input)) return defaultValue;
            return input.StartsWith("y");
        }

        public static int PromptInt(string label, int defaultValue)
        {
            System.Console.Write($"{label} [{defaultValue}]: ");
            var input = System.Console.ReadLine();
            return int.TryParse(input, out var result) ? result : defaultValue;
        }
    }
}
