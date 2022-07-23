namespace KanBanApp.Commands;

[Command("open")]
public class Open : CommandBase
{
    [Argument(0, "Path")]
    public string Path { get; set; }
    
    protected override int OnExecute()
    {
        return 0;
    }
}