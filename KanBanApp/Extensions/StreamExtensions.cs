namespace KanBanApp.Extensions;

internal static class StreamExtensions
{
    public static void AssertCanRead(this Stream stream, string appendMessage = "")
    {
        appendMessage = appendMessage.Length == 0 ? string.Empty : " " + appendMessage;
        
        if (!stream.CanRead)
            throw new ArgumentException($"Stream read is denied.{appendMessage}");
    }
    
    public static void AssertCanWrite(this Stream stream, string appendMessage = "")
    {
        appendMessage = appendMessage.Length == 0 ? string.Empty : " " + appendMessage;
        
        if (!stream.CanWrite)
            throw new ArgumentException($"Stream write is denied.{appendMessage}");
    }

    public static bool AtEnd(this Stream stream)
    {
        return stream.Position == stream.Length;
    }
}