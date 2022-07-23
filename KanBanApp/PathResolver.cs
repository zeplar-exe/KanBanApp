namespace KanBanApp;

public static class PathResolver
{
    public static string RelativeOrAbsoluteFilePath(string? path)
    {
        if (!string.IsNullOrEmpty(path))
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