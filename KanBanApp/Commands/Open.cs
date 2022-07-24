using System.ComponentModel.DataAnnotations;
using KanBanApp.Projects;

namespace KanBanApp.Commands;

[Command("open")]
public class Open : CommandBase
{
    [Argument(0, "Path")]
    [Required]
    public string Path { get; set; }
    
    protected override int Execute()
    {
        if (!TestProjectExists(out var project))
            return 1;

        if (!ObjectPath.TryParse(Path, out var openPath))
        {
            WriteOutputLine($"The path '{Path}' is invalid (bad format).");
            
            return 1;
        }

        project.Session.OpenPath = openPath;
        project.Write();
        
        return 0;
    }
}