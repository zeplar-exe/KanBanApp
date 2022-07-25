using System.Xml.Linq;
using KanBanApp.Extensions;
using KanBanApp.Helpers;

namespace KanBanApp.Projects.Objects;

public class BoardList : DescribedDirectoryObject
{
    public List<ListCard> Cards { get; }

    public BoardList()
    {
        Cards = new List<ListCard>();
    }
    
    public override void FromDirectory(string path)
    {
        DirectoryHelper.AssertExists(path);
        
        var contentsStream = FileHelper.OpenReadXmlOrEmpty(Path.Join(path, "contents.xml"));
        var contentsDocument = XDocument.Load(contentsStream);
        
        var contentLoader = new ContentLoader(contentsDocument.Root!);

        var cardsDirectory = Path.Join(path, "cards");
        FileSystemHelper.EnsureDirectory(cardsDirectory);
        
        var cards = DescribedObject.FromContent<ListCard>(
            cardsDirectory, contentLoader, "cards");
        
        Cards.AddRange(cards);
    }

    public override void WriteDirectory(string path)
    {
        Directory.CreateDirectory(path);
        
        var contentsStream = File.OpenWrite(Path.Join(path, "contents.xml"));
        WriteXml().Save(contentsStream);

        var listsDirectory = Directory.CreateDirectory(Path.Join(path, "cards"));

        foreach (var card in Cards)
        {
            card.WriteXml().Save(Path.Join(listsDirectory.FullName, card.Name));
        }
    }

    public BoardList Copy()
    {
        var list = new BoardList { Name = Name, Description = Description };
        Fields.CopyTo(list.Fields);
        Cards.CopyTo(list.Cards);

        return list;
    }
}