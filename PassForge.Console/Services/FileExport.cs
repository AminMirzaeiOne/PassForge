namespace PassForge.Console.Services
{
    public static class FileExport
    {
        public static void ExportToFile(IEnumerable<string> list, string path)
        {
            File.WriteAllLines(path, list);
            ConsoleUI.WriteInfo($"Passwords exported to: {path}");
        }
    }
}
