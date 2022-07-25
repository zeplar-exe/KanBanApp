using System.ComponentModel.DataAnnotations;

namespace KanBanApp.Commands;

[Command("config")]
public class Config : CommandBase
{
    [Option("-l|--list", CommandOptionType.NoValue)]
    public bool List { get; set; }
    
    [Option("-g|--global", CommandOptionType.NoValue)]
    public bool Global { get; set; }
    
    [Option("-c|--custom", CommandOptionType.NoValue)]
    public bool Custom { get; set; }
    
    [Option("-d|--del", CommandOptionType.NoValue)]
    public bool Delete { get; set; }

    [Argument(0, "Configuration")]
    public string? Configuration { get; set; }
    
    [Argument(1, "Value")]
    public string? Value { get; set; }

    protected override int Execute()
    {
        if (!TestProjectExists(out var project))
            return 1;
        
        if (List)
        {
            foreach (var property in project.Configuration.EnumerateProperties())
            {
                WriteOutputLine($"{property.Name}={property.GetValue(project.Configuration)}");
            }

            foreach (var custom in project.Configuration.Custom)
            {
                WriteOutputLine($"(Custom) {custom.Key}={custom.Value}");
            }

            return 0;
        }

        if (Configuration == null)
        {
            WriteOutputLine($"Argument 0, 'Configuration', is required (except for --list).");

            return 1;
        }
        
        Value ??= string.Empty;

        if (Custom)
        {
            if (Delete)
            {
                if (project.Configuration.Custom.Remove(Configuration))
                {
                    WriteOutputLine($"Deleted '{Configuration}'.");
                    
                    project.Write();
                    
                    return 0;
                }
                else
                {
                    WriteOutputLine($"The custom configuration '{Configuration}' does not exist.");

                    return 1;
                }
            }
            project.Configuration.Custom[Configuration] = Value;
        }
        else if (project.Configuration.TrySet(Configuration, Value))
        {
            if (Delete)
            {
                WriteOutputLine("Cannot delete a builtin configuration. Did you mean to use --custom?");

                return 1;
            }
        }
        else
        {
            WriteOutputLine($"The configuration '{Configuration}' does not exist. Did you mean to use --custom?");

            return 1;
        }
        
        WriteOutputLine($"{Configuration}={Value}");
        
        project.Write();

        return 0;
    }
}