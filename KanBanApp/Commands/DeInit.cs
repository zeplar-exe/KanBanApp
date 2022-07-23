using KanBanApp.Projects;

namespace KanBanApp.Commands;

[Command("deinit")]
public class DeInit : CommandBase
{
    [Option("-d|--dir <DirectoryPath>", CommandOptionType.SingleValue)]
    public string? DirectoryPath { get; set; }

    protected override int Execute()
    {
        var target = PathResolver.RelativeOrAbsoluteFilePath(DirectoryPath);
        var directory = new DirectoryInfo(target);

        if (!directory.Exists)
        {
            WriteOutputLine($"The directory '{target}' does not exist.");
            
            return 1;
        }

        if (directory.Name == ".kba")
        {
            directory.Delete();
            
            WriteOutputLine($"Successfully deleted '{target}'.");

            return 0;
        }

        var subDirectory = new DirectoryInfo(Path.Join(target, ProjectInterface.MetaDirectoryName));

        if (!subDirectory.Exists)
        {
            WriteOutputLine($"A .kba project does not exist within '{target}'.");

            return 1;
        }
        
        subDirectory.Delete();
        
        return 0;
    }
}