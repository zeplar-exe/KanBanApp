using System.Diagnostics.CodeAnalysis;
using System.Xml;
using ExtendedXmlSerializer;
using ExtendedXmlSerializer.Configuration;

namespace KanBanApp.Projects;

public class AppConfiguration
{
    public bool AutoCreatePathParts { get; set; }

    public Dictionary<string, string> Custom { get; set; }

    public static AppConfiguration FromXml(Stream stream)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Stream read is denied. Configuration deserialization aborted.");
        
        var serializer = new ConfigurationContainer().Create();
        var result = serializer.Deserialize<AppConfiguration>(stream);

        return result;
    }

    public void WriteXml(Stream stream)
    {
        if (!stream.CanWrite)
            throw new ArgumentException("Stream write is denied. Configuration serialization aborted.");
        
        var serializer = new ConfigurationContainer().Create();
        var result = serializer.Serialize(
            new XmlWriterSettings { Indent = true },
            this);

        using var writer = new StreamWriter(stream);
        
        writer.Write(result);
    }

    public bool TrySet(string config, string value)
    {
        var property = GetType().GetProperty(config);

        if (property == null)
            return false;

        if (!TryConvert(value, property.PropertyType, out var converted))
            return false;
        
        property.SetValue(this, converted);

        return true;

    }

    private bool TryConvert(string input, Type targetType, [NotNullWhen(true)] out object? converted)
    {
        bool result;
        
        if (targetType == typeof(int))
        {
            result = int.TryParse(input, out var i);
            converted = i;
        }
        else if (targetType == typeof(string))
        {
            result = true;
            converted = input;
        }
        else if (targetType == typeof(bool))
        {
            result = bool.TryParse(input, out var b);
            converted = b;
        }
        else
        {
            result = false;
            converted = null;
        }

        return result;
    }
}