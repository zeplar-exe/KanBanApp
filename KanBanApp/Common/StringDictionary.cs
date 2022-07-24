using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace KanBanApp.Common;

[Serializable]
public class StringDictionary : Dictionary<string, string>, IXmlSerializable
{
    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        if (reader.IsEmptyElement)
        {
            return;
        }
        
        if (!reader.Read())
        {
            throw new XmlException("Input XML is invalid (bad read).");
        }
        
        while (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.ReadStartElement();
            
            var key = reader.Name;
            var value = reader.Value;
            
            reader.ReadEndElement();
            
            this[key] = value;
            
            // reader.MoveToContent();
        }
        
        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var pair in this)
        {
            writer.WriteStartElement(pair.Key);
            writer.WriteValue(pair.Value);
            writer.WriteEndAttribute();
        }
    }
}