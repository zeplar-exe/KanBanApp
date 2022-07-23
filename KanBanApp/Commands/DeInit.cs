namespace KanBanApp.Commands;

[Command("deinit")]
public class DeInit : CommandBase
{
    [Option("-d|--dir <DirectoryPath>", CommandOptionType.SingleValue)]
    public string? DirectoryPath { get; set; }

    protected override int OnExecute()
    {
        var target = PathResolver.RelativeOrAbsoluteFilePath(DirectoryPath);
        var directory = new DirectoryInfo(target);

        if (!directory.Exists)
        {
            WriteOutputLine($"The directory '{target}' does not exist.");
            
            return Exit(1);
        }

        if (directory.Name == ".kba")
        {
            directory.Delete();
            
            WriteOutputLine($"Successfully deleted '{target}'.");

            return Exit(0);
        }

        var subDirectory = new DirectoryInfo(Path.Join(target, ProjectInterface.MetaDirectoryName));

        if (!subDirectory.Exists)
        {
            WriteOutputLine($"A .kba project does not exist within '{target}'.");

            return Exit(1);
        }
        
        subDirectory.Delete();

        return Exit(0);
    }
}