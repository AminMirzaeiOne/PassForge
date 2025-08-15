namespace PassForge.ConsoleApp.Services
{
    public static class UserInput
    {
        public static string Prompt(string label, bool allowEmpty = false, string? placeholder = null)
        {
            System.Console.Write(label + (placeholder is null ? "" : $" ({placeholder})") + ": ");
            var input = System.Console.ReadLine()?.Trim() ?? "";
            if (!allowEmpty)
            {
                while (string.IsNullOrWhiteSpace(input))
                {
                    System.Console.Write("لطفاً مقدار وارد کنید: ");
                    input = System.Console.ReadLine()?.Trim() ?? "";
                }
            }
            return input;
        }

        public static int PromptInt(string label, int min, int max, int? defaultValue = null)
        {
            while (true)
            {
                System.Console.Write(label + $" ({min}-{max}" + (defaultValue.HasValue ? $", پیشفرض={defaultValue}" : "") + "): ");
                var input = System.Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input) && defaultValue.HasValue) return defaultValue.Value;
                if (int.TryParse(input, out var value) && value >= min && value <= max) return value;
                System.Console.WriteLine("مقدار نامعتبر است.");
            }
        }

        public static bool PromptYesNo(string label, bool? defaultYes = null)
        {
            var hint = defaultYes == null ? "" : defaultYes.Value ? " [Y/n]" : " [y/N]";
            System.Console.Write(label + hint + ": ");
            var input = System.Console.ReadLine()?.Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(input) && defaultYes.HasValue) return defaultYes.Value;
            return input is "y" or "yes" or "بله";
        }

        public static string PromptSet(string label, string current)
        {
            System.Console.Write($"{label} (فعلی: \"{current}\"): ");
            var s = System.Console.ReadLine()?.Trim() ?? "";
            return string.IsNullOrEmpty(s) ? current : s;
        }

        public static string[] PromptList(string label, string[] current, string hint = "با کاما جدا کنید")
        {
            System.Console.Write($"{label} ({hint}) (فعلی: {string.Join(",", current)}): ");
            var s = System.Console.ReadLine()?.Trim() ?? "";
            return string.IsNullOrEmpty(s)
                ? current
                : s.Split(',').Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
        }
    }
}
