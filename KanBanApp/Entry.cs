using KanBanApp.Commands;

namespace KanBanApp;

[Command("kba")]
[Subcommand(typeof(Open))]
public class Entry
{
    private void OnExecute()
    {
        
    }
}