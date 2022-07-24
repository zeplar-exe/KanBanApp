using KanBanApp.Commands;

namespace KanBanApp;

[Command("kba")]
[Subcommand(typeof(Init), typeof(DeInit), typeof(Open), typeof(Print))]
public class Entry
{
    private void OnExecute()
    {
        
    }
}