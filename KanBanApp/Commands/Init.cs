using KanBanApp.Projects;

namespace KanBanApp.Commands;

[Command("init")]
public class Init : CommandBase
{
    [Option("-d|--dir <DirectoryPath>", CommandOptionType.SingleValue)]
    public string? DirectoryPath { get; set; }
    
    [Option("-f|--force", CommandOptionType.NoValue)]
    public bool Force { get; set; }

    protected override int Execute()
    {
        var target = PathResolver.RelativeOrAbsoluteFilePath(DirectoryPath);

        if (ProjectInterface.ExistsIn(target))
        {
            if (!Force)
            {
                WriteOutputLine("A kba project already exists in this directory. Use --force to overwrite.");

                return 1;
            }
        }
        
        var board = new ProjectInterface(target);
        
        board.Write();
        
        WriteOutputLine($"Initialized a kba project in '{target}'.");

        return 0;
    }
}