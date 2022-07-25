using System.Xml.Linq;
using KanBanApp.Extensions;

namespace KanBanApp.Projects.Objects;

public class ListCard : DescribedObject
{
    public string Content { get; set; }

    public ListCard()
    {
        Content = string.Empty;
    }

    public override void ExtendFromXml(XElement root)
    {
        Content = root.Element("Name")?.Value ?? string.Empty;
    }

    public override void ExtendWriteXml(XElement element)
    {
        element.Add(new XElement("Content", Content));
    }
    
    public ListCard Copy()
    {
        var card = new ListCard { Name = Name, Description = Description, Content = Content };
        Fields.CopyTo(card.Fields);

        return card;
    }
}