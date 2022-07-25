using KanBanApp.Extensions;

namespace KanBanApp.Helpers;

internal static class FileHelper
{
    public static Stream OpenReadXmlOrEmpty(string path)
    {
        return OpenOrDefault(path, "<root></root>");
    }
    
    public static Stream OpenOrDefault(string path, string text)
    {
        if (File.Exists(path))
        {
            var stream = File.OpenRead(path);

            if (!stream.AtEnd())
                return stream;
        }
        
        var memory = new MemoryStream();

        var writer = new StreamWriter(memory, leaveOpen: true);
        writer.Write(text);

        return memory;
    }
}