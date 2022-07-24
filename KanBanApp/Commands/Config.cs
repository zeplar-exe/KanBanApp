using System.ComponentModel.DataAnnotations;

namespace KanBanApp.Commands;

[Command("config")]
public class Config : CommandBase
{
    [Option("-g|--global", CommandOptionType.NoValue)]
    public bool Global { get; set; }
    
    [Option("-c|--custom", CommandOptionType.NoValue)]
    public bool Custom { get; set; }

    [Argument(0, "Configuration")]
    [Required]
    public string Configuration { get; set; }
    
    [Argument(1, "Value")]
    public string? Value { get; set; }

    protected override int Execute()
    {
        Value ??= string.Empty;
        
        if (!TestProjectExists(out var project))
            return 1;

        if (Custom)
        {
            project.Configuration.Custom[Configuration] = Value;
        }
        else
        {
            if (project.Configuration.TrySet(Configuration, Value))
            {
                WriteOutputLine($"{Configuration}={Value}");
            }
            else
            {
                WriteOutputLine($"The configuration '{Configuration}' does not exist. Did you mean to use --custom?");

                return 1;
            }
        }

        return 0;
    }
}