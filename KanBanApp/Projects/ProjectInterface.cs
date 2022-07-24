using System.Xml;
using System.Xml.Linq;

namespace KanBanApp.Projects;

public class ProjectInterface
{
    public const string MetaDirectoryName = ".kba";
    
    private DirectoryInfo MetaDirectory { get; }
    
    public SessionInfo Session { get; set; }
    
    public ProjectInterface(string directory)
    {
        MetaDirectory = new DirectoryInfo(Path.Join(directory, MetaDirectoryName));

        Session = new SessionInfo();
    }

    public static bool ExistsIn(string directory)
    {
        return Directory.Exists(Path.Join(directory, MetaDirectoryName));
    }

    public void Update()
    {
        if (!MetaDirectory.Exists)
            return;
        
        var sessionFile = Path.Join(MetaDirectory.FullName, "session.xml");
        EnsureFile(sessionFile);

        using var stream = File.OpenRead(sessionFile);
        var file = XDocument.Load(stream);

        Session = SessionInfo.FromXml(file);
    }

    public void Write()
    {
        if (MetaDirectory.Exists)
            MetaDirectory.Delete(true);
        
        MetaDirectory.Create();
        MetaDirectory.Attributes |= FileAttributes.Hidden;

        var sessionDocument = new XDocument(new XElement("root"));
        var sessionFile = Path.Join(MetaDirectory.FullName, "session.xml");
        
        Session.WriteXml(sessionDocument.Root);
        
        sessionDocument.Save(sessionFile);
    }

    private void EnsureFile(string path)
    {
        if (!File.Exists(path))
            File.Create(path).Dispose();
    }
}