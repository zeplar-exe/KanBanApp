using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using KanBanApp.Common;
using KanBanApp.Extensions;

namespace KanBanApp.Projects;

[XmlRoot("root")]
public class AppConfiguration
{
    public bool AutoCreatePathParts { get; set; }
    
    public StringDictionary Custom { get; set; }

    public AppConfiguration()
    {
        Custom = new StringDictionary();
    }

    public static AppConfiguration FromXml(Stream stream)
    {
        stream.AssertCanRead("Configuration deserialization aborted.");
        
        var serializer = new XmlSerializer(typeof(AppConfiguration));
        var result = serializer.Deserialize(stream) as AppConfiguration ?? new AppConfiguration();

        return result;
    }

    public void WriteXml(Stream stream)
    {
        stream.AssertCanWrite("Configuration serialization aborted.");
     
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        
        var serializer = new XmlSerializer(typeof(AppConfiguration));
        serializer.Serialize(stream, this, ns);
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