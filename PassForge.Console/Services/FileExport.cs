namespace PassForge.Console.Services
{
    public static class FileExport
    {
        public static void ExportToFile(IEnumerable<string> passwords, string filePath)
        {
            File.WriteAllLines(filePath, passwords);
            PassForge.Console.Services.ConsoleUI.WriteSuccess($"لیست پسورد در فایل ذخیره شد: {filePath}");
        }
    }
}
