using System.Text;
using System.Xml.Linq;
using KanBanApp.Extensions;
using KanBanApp.Helpers;
using KanBanApp.Projects.Objects;

namespace KanBanApp.Projects;

public class ProjectInterface
{
    public const string MetaDirectoryName = ".kba";

    private DirectoryInfo MetaDirectory { get; }

    public SessionInfo Session { get; set; }
    public AppConfiguration Configuration { get; set; }

    public List<ProjectBoard> Boards { get; }

    public ProjectInterface(string directory)
    {
        MetaDirectory = new DirectoryInfo(Path.Join(directory, MetaDirectoryName));

        Session = new SessionInfo();
        Configuration = new AppConfiguration();
        Boards = new List<ProjectBoard>();
    }

    public static bool ExistsIn(string directory)
    {
        return Directory.Exists(Path.Join(directory, MetaDirectoryName));
    }

    public void Update()
    {
        if (!MetaDirectory.Exists)
            throw new InvalidOperationException(
                $"A kba directory does not exist within '{MetaDirectory.Parent?.FullName}'. Initialization aborted.");

        using var sessionStream = FileHelper.OpenReadXmlOrEmpty(GetPathFromMeta("session.xml"));
        Session = SessionInfo.FromXml(sessionStream);

        using var configStream = FileHelper.OpenReadXmlOrEmpty(GetPathFromMeta("config.xml"));
        Configuration = AppConfiguration.DeserializeFromXml(configStream);
        
        LoadContent();
    }

    private void LoadContent()
    {
        using var contentsStream = FileHelper.OpenReadXmlOrEmpty(GetPathFromMeta("contents.xml"));
        var document = XDocument.Load(contentsStream);
        
        var contentLoader = new ContentLoader(document.Root!);

        var boardsDirectory = Path.Join(MetaDirectory.FullName, "boards");
        FileSystemHelper.EnsureDirectory(boardsDirectory);
        
        var directoryObjects = DescribedDirectoryObject.FromContent<ProjectBoard>(
            boardsDirectory, contentLoader, "boards");
        
        Boards.AddRange(directoryObjects);
    }

    public void Write()
    {
        if (MetaDirectory.Exists)
            MetaDirectory.Delete(true);
        
        MetaDirectory.Create();
        MetaDirectory.Attributes |= FileAttributes.Hidden;
        
        using var sessionStream = OpenWriteStream("session.xml");
        Session.WriteXml(sessionStream);

        using var configStream = OpenWriteStream("config.xml");
        Configuration.WriteXml(configStream);

        WriteContent();
    }

    private void WriteContent()
    {
        using var contentsStream = OpenWriteStream("contents.xml");
        var contentsRoot = new XElement("root");
        var contentsDocument = new XDocument(contentsRoot);

        var boardsElement = new XElement("boards");
        var boardDirectory = Path.Join(MetaDirectory.FullName, "boards");

        foreach (var board in Boards)
        {
            boardsElement.Add(board.WriteXml());
            board.WriteDirectory(Path.Join(boardDirectory, board.Name));
        }
        
        contentsRoot.Add(boardsElement);
        
        contentsDocument.Save(contentsStream);
    }

    private string GetPathFromMeta(string relative)
    {
        return Path.Join(MetaDirectory.FullName, relative);
    }

    private Stream OpenWriteStream(string name)
    {
        return File.OpenWrite(Path.Join(MetaDirectory.FullName, name));
    }
}