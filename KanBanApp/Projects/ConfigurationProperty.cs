namespace KanBanApp.Projects;

[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationProperty : Attribute
{
    public string? Name { get; }

    public ConfigurationProperty(string? name = null)
    {
        Name = name;
    }
}