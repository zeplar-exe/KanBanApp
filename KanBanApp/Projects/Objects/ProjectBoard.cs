using System.Xml.Linq;
using KanBanApp.Extensions;
using KanBanApp.Helpers;

namespace KanBanApp.Projects.Objects;

public class ProjectBoard : DescribedDirectoryObject
{
    public List<BoardList> Lists { get; }

    public ProjectBoard()
    {
        Lists = new List<BoardList>();
    }

    public override void FromDirectory(string path)
    {
        DirectoryHelper.AssertExists(path);
        
        var contentsStream = FileHelper.OpenReadXmlOrEmpty(Path.Join(path, "contents.xml"));
        var contentsDocument = XDocument.Load(contentsStream);
        
        var contentLoader = new ContentLoader(contentsDocument.Root!);
        
        var listsDirectory = Path.Join(path, "lists");
        FileSystemHelper.EnsureDirectory(listsDirectory);
        
        var lists = FromContent<BoardList>(
            listsDirectory, contentLoader, "lists");
        
        Lists.AddRange(lists);
    }

    public override void WriteDirectory(string path)
    {
        Directory.CreateDirectory(path);
        
        var contentsStream = File.OpenWrite(Path.Join(path, "contents.xml"));
        WriteXml().Save(contentsStream);

        var listsDirectory = Directory.CreateDirectory(Path.Join(path, "lists"));

        foreach (var list in Lists)
        {
            var listDirectory = listsDirectory.CreateSubdirectory(list.Name);
            
            list.WriteDirectory(listDirectory.FullName);
        }
    }
    
    public ProjectBoard Copy()
    {
        var board = new ProjectBoard { Name = Name, Description = Description };
        Fields.CopyTo(board.Fields);
        Lists.CopyTo(board.Lists);

        return board;
    }
}