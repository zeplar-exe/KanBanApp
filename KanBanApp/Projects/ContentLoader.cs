using System.Xml.Linq;
using KanBanApp.Projects.Objects;

namespace KanBanApp.Projects;

public class ContentLoader
{
    private XElement XmlRoot { get; }

    public ContentLoader(XElement root)
    {
        XmlRoot = root;
    }

    public IEnumerable<TObject> Load<TObject>(string elementName) where TObject : DescribedObject, new()
    {
        var targetElement = XmlRoot.Element(elementName);
        
        if (targetElement == null)
            yield break;

        foreach (var element in targetElement.Elements())
        {
            var o = new TObject();
            o.FromXml(element);

            yield return o;
        }
    }
}