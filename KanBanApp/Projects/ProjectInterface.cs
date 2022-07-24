using System.Xml.Linq;

namespace KanBanApp.Projects;

public class ProjectInterface
{
    public const string MetaDirectoryName = ".kba";
    
    private DirectoryInfo MetaDirectory { get; }
    
    public SessionInfo Session { get; set; }
    public AppConfiguration Configuration { get; set; }
    
    public ProjectInterface(string directory)
    {
        MetaDirectory = new DirectoryInfo(Path.Join(directory, MetaDirectoryName));

        Session = new SessionInfo();
        Configuration = new AppConfiguration();
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

        using var sessionStream = OpenReadStream("session.xml");
        Session = SessionInfo.FromXml(sessionStream);
        
        using var configStream = OpenReadStream("config.xml");
        Configuration = AppConfiguration.FromXml(configStream);
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
    }
    
    private Stream OpenReadStream(string name)
    {
        var file = Path.Join(MetaDirectory.FullName, name);
        EnsureFile(file);

        return File.OpenRead(file);
    }

    private Stream OpenWriteStream(string name)
    {
        return File.OpenWrite(Path.Join(MetaDirectory.FullName, name));
    }

    private void EnsureFile(string path)
    {
        if (!File.Exists(path))
            File.Create(path).Dispose();
    }
}