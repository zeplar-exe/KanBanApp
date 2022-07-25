using System.Xml.Linq;
using KanBanApp.Common;
using KanBanApp.Helpers;

namespace KanBanApp.Projects.Objects;

public abstract class DescribedObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public StringDictionary Fields { get; }

    public DescribedObject()
    {
        Name = string.Empty;
        Description = string.Empty;
        Fields = new StringDictionary();
    }
    
    public static IEnumerable<TObject> FromContent<TObject>(string contentDirectory, ContentLoader loader, string elementName) 
        where TObject : DescribedObject, new()
    {
        foreach (var item in loader.Load<TObject>(elementName))
        {
            var file = Path.Join(contentDirectory, item.Name + ".xml");
            FileSystemHelper.EnsureFile(file);

            var stream = FileHelper.OpenReadXmlOrEmpty(file);
            var document = XDocument.Load(stream);
            
            item.FromXml(document.Root!);

            yield return item;
        }
    }

    public void FromXml(XElement element)
    {
        Name = element.Element("Name")?.Value ?? string.Empty;
        Description = element.Element("Description")?.Value ?? string.Empty;

        foreach (var field in element.Element("Fields")?.Elements() ?? Enumerable.Empty<XElement>())
        {
            Fields[field.Name.ToString()] = field.Value;
        }
        
        ExtendFromXml(element);
    }
    
    public virtual void ExtendFromXml(XElement root) { }
    
    public XElement WriteXml()
    {
        var hash = StringHash.Hash(Name);
        var id = hash.ToString();
        
        var element = new XElement(id);
        
        element.Add(new XElement("Name", Name));
        element.Add(new XElement("Description", Description));

        var fieldElement = new XElement("Fields");
        
        foreach (var field in Fields)
        {
            fieldElement.Add(new XElement(field.Key, field.Value));
        }
        
        ExtendWriteXml(element);

        return element;
    }

    public virtual void ExtendWriteXml(XElement element) { }
}