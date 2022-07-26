using System.ComponentModel.DataAnnotations;

namespace KanBanApp.Commands;

public class Edit : CommandBase
{
    [Argument(0, "Path")]
    [Required]
    public string Path { get; set; }

    protected override int Execute()
    {
        return 0;
    }
}