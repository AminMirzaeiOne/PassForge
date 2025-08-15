namespace PassForge.Console.Views
{
    public static class PasswordPreview
    {
        public static void Show(IEnumerable<string> list, int previewCount = 20)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Generated Passwords (Preview):");
            System.Console.WriteLine("-------------------------------");

            foreach (var pwd in list.Take(previewCount))
                System.Console.WriteLine(pwd);

            if (list.Count() > previewCount)
            {
                System.Console.WriteLine($"... and {list.Count() - previewCount} more");
            }
            System.Console.WriteLine();
        }
    }
}
