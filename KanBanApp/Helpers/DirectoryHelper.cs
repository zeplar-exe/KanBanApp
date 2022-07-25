namespace KanBanApp.Helpers;

internal static class DirectoryHelper
{
    public static void AssertExists(DirectoryInfo directory, string appendMessage = "")
    {
        AssertExists(directory.FullName, appendMessage);
    }
    
    public static void AssertExists(string path, string appendMessage = "")
    {
        appendMessage = appendMessage.Length == 0 ? string.Empty : " " + appendMessage;
        
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException($"Directory '{path}' does not exist.{appendMessage}");
    }
}