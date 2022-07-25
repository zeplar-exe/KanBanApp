namespace KanBanApp.Helpers;

internal static class FileSystemHelper
{
    public static void EnsureFile(string path)
    {
        if (!File.Exists(path))
            File.Create(path).Dispose();
    }

    public static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}