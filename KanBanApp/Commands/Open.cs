namespace KanBanApp.Commands;

[Command("open")]
public class Open
{
    [Argument(0, "Path")]
    public string Path { get; set; }
    
    private void OnExecute()
    {
        
    }
}