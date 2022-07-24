using System.ComponentModel.DataAnnotations;
using KanBanApp.Common;

namespace KanBanApp.Commands;

public class Print : CommandBase
{
    [Option("--pretty", CommandOptionType.NoValue)]
    public bool Pretty { get; set; }
    
    [Option("-e|--echo", CommandOptionType.NoValue)]
    public bool Echo { get; set; }

    [Argument(0, "Target")]
    [Required]
    public string PrintTarget { get; set; }
    
    protected override int Execute()
    {
        if (Echo)
        {
            WriteOutputLine(PrintTarget);

            return 0;
        }
        
        return EvaluateTarget();
    }

    private int EvaluateTarget()
    {
        if (string.IsNullOrWhiteSpace(PrintTarget))
            return 1;
        
        if (!TestProjectExists(out var project))
            return 1;

        var split = PrintTarget.Split('.');
        var arraySwitch = new ArraySwitch<string>();

        arraySwitch.Register("session", "OpenPath").As(() =>
        {
            WriteOutputLine(project.Session.OpenPath.ToString());
        });

        arraySwitch.Register("config", "xml").As(() =>
        {
            var stream = new MemoryStream();
            project.Configuration.WriteXml(stream);

            using var reader = new StreamReader(stream);
            
            WriteOutputLine(reader.ReadToEnd());
        });

        if (arraySwitch.Try(split))
        {
            return 0;
        }
        else
        {
            WriteOutputLine($"The print target '{PrintTarget}' is not valid.");

            return 1;
        }
    }
}