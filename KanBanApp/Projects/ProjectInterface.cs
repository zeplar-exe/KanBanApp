namespace KanBanApp.Projects;

public class ProjectInterface
{
    public const string MetaDirectoryName = ".kba";
    
    private DirectoryInfo MetaDirectory { get; }
    
    public ProjectInterface(string directory)
    {
        MetaDirectory = new DirectoryInfo(Path.Join(directory, MetaDirectoryName));
    }

    public void Update()
    {
        
    }

    public void Write()
    {
        if (MetaDirectory.Exists)
            MetaDirectory.Delete(true);
        
        MetaDirectory.Create();
        MetaDirectory.Attributes |= FileAttributes.Hidden;
    }
}