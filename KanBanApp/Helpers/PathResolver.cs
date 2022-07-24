namespace KanBanApp.Helpers;

public static class PathResolver
{
    public static string RelativeOrAbsoluteFilePath(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            if (Path.IsPathRooted(path) || Path.IsPathFullyQualified(path))
            {
                return path;
            }
            
            return Path.Join(Directory.GetCurrentDirectory(), path);
        }

        return Directory.GetCurrentDirectory();
    }
}