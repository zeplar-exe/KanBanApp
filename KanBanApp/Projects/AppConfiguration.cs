using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml.Serialization;
using KanBanApp.Common;
using KanBanApp.Extensions;

namespace KanBanApp.Projects;

[XmlRoot("root")]
public class AppConfiguration
{
    [ConfigurationProperty]
    public bool AutoCreatePathParts { get; set; }
    
    public StringDictionary Custom { get; set; }

    public AppConfiguration()
    {
        Custom = new StringDictionary();
    }

    public static AppConfiguration DeserializeFromXml(Stream stream)
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

    public bool TryGet(string config, out object? value)
    {
        const StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;

        value = null;
        
        var property = EnumerateProperties()
            .FirstOrDefault(f => f.ResolveName().Equals(config, comparison));

        if (property == null)
            return false;

        value = property.PropertyInfo.GetValue(this);

        return true;
    }

    public bool TrySet(string config, string value)
    {
        const StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;
        
        var property = EnumerateProperties()
            .FirstOrDefault(f => f.ResolveName().Equals(config, comparison));

        if (property == null)
            return false;

        if (!TryConvert(value, property.PropertyInfo.PropertyType, out var converted))
            return false;
        
        property.PropertyInfo.SetValue(this, converted);

        return true;
    }

    public IEnumerable<PropertyItem> EnumerateProperties()
    {
        return GetType().GetProperties()
            .Where(p => p.GetCustomAttribute<ConfigurationProperty>() != null)
            .Select(p => new PropertyItem(p, p.GetCustomAttribute<ConfigurationProperty>()!));
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

    public record PropertyItem(PropertyInfo PropertyInfo, ConfigurationProperty Attribute)
    {
        public string ResolveName()
        {
            return Attribute.Name ?? PropertyInfo.Name;
        }
    }
}