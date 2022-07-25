using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using KanBanApp.Common;
using KanBanApp.Projects;

namespace KanBanApp.Commands;

public class Print : CommandBase
{
    [Option("--pretty", CommandOptionType.NoValue)]
    public bool Pretty { get; set; }
    
    [Option("--list-targets", CommandOptionType.NoValue)]
    public bool ListTargets { get; set; }
    
    [Option("-e|--echo", CommandOptionType.NoValue)]
    public bool Echo { get; set; }

    [Argument(0, "Target")]
    public string? PrintTarget { get; set; }
    
    protected override int Execute()
    {
        if (ListTargets)
        {
            foreach (var target in EnumerateTargets())
            {
                WriteOutputLine(string.Join('.', target.Keys));
            }

            return 0;
        }

        if (PrintTarget == null)
        {
            WriteOutputLine($"Argument 0, 'Target', is required (except for --list).");

            return 1;
        }
        
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
        
        AssertProjectExists(out var project);

        var split = PrintTarget.Split('.');
        var arraySwitch = new ArraySwitch<string>();

        foreach (var target in EnumerateTargets())
        {
            arraySwitch.Register(target.Keys).As(target.Action ?? (() => {}));
        }

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

    private IEnumerable<Target> EnumerateTargets()
    {
        AssertProjectExists(out var project);
        
        yield return Target.Create("session", "OpenPath").As(() =>
        {
            WriteOutputLine(project.Session.OpenPath.ToString());
        });

        yield return Target.Create("config", "xml").As(() =>
        {
            var stream = new MemoryStream();
            project.Configuration.WriteXml(stream);
            
            stream.Flush();
            stream.Position = 0;

            using var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();

            if (Pretty)
            {
                var document = XDocument.Parse(xml);
                
                WriteOutputLine(document.ToString());
            }
            else
            {
                WriteOutputLine(xml);   
            }
        });
    }

    private class Target
    {
        public string[] Keys { get; }
        public Action? Action { get; set; }

        private Target(string[] keys)
        {
            Keys = keys;
        }

        public static TargetRegister Create(params string[] keys)
        {
            var target = new Target(keys);

            return new TargetRegister(target);
        }

        public class TargetRegister
        {
            private Target Target { get; }

            public TargetRegister(Target target)
            {
                Target = target;
            }

            public Target As(Action action)
            {
                Target.Action = action;

                return Target;
            }
        }
    }
}