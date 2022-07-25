using System.Security.Cryptography;
using System.Text;

namespace KanBanApp.Common;

public class StringHash
{
    public static Encoding Encoding { get; } = Encoding.UTF8; 
    
    public byte[] Bytes { get; }

    private StringHash(byte[] bytes)
    {
        Bytes = bytes;
    }
    
    public static StringHash Hash(string input)
    {
        using var sha = SHA256.Create();
        var nameBytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(nameBytes);

        return new StringHash(hash);
    }

    public override string ToString()
    {
        return Convert.ToHexString(Bytes);
    }
}