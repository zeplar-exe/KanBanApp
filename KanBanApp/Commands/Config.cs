using System.ComponentModel.DataAnnotations;

namespace KanBanApp.Commands;

[Command("config")]
public class Config : CommandBase
{
    [Option("-l|--list", CommandOptionType.NoValue)]
    public bool List { get; set; }
    
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
        AssertProjectExists(out var project);
        
        if (List)
        {
            foreach (var property in project.Configuration.EnumerateProperties())
            {
                var value = property.PropertyInfo.GetValue(project.Configuration);
                
                WriteOutputLine($"{property.ResolveName()}={value}");
            }

            foreach (var custom in project.Configuration.Custom)
            {
                WriteOutputLine($"(Custom) {custom.Key}={custom.Value}");
            }

            return 0;
        }

        if (Configuration == null)
        {
            WriteOutputLine("Argument 0, 'Configuration', is required (except for --list).");

            return 1;
        }

        if (Value == null && !Delete)
        {
            if (project.Configuration.TryGet(Configuration, out var configValue))
            {
                WriteOutputLine(SingleConfigDisplay(Configuration, configValue.ToString()));

                return 0;
            }
            else
            {
                if (Custom)
                {
                    if (project.Configuration.Custom.TryGetValue(Configuration, out var customValue))
                    {
                        WriteOutputLine(SingleConfigDisplay(Configuration, customValue));
                    
                        project.Write();
                    
                        return 0;
                    }
                    else
                    {
                        WriteOutputLine($"The custom configuration '{Configuration}' does not exist.");

                        return 1;
                    }
                }
                
                WriteOutputLine($"The configuration '{Configuration}' does not exist. Did you mean to use --custom?");
                
                return 1;
            }
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
        
        WriteOutputLine(SingleConfigDisplay(Configuration, Value));
        
        project.Write();

        return 0;
    }

    private string SingleConfigDisplay(string name, string? value)
    {
        return $"{name}={value ?? "null"}";
    }
}