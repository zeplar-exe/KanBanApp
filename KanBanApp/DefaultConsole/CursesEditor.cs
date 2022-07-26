namespace KanBanApp.DefaultConsole;

public class CursesEditor
{
    public const string SectionSeparator = "\fNEW_SECTION\f";
    
    private bool Exited { get; set; }

    public string[] Sections { get; }

    public CursesEditor(string[] sections)
    {
        Sections = sections;
    }
    
    public string Run()
    {
        try
        {
            while (!Exited)
            {
                Render();
                HandleInput();
            }
        }
        finally
        {
            Console.Clear();
            
            Exited = false;
        }

        return string.Empty;
    }

    private void Render()
    {
        Console.Clear();
        
        for (var index = 0; index < Sections.Length; index++)
        {
            var section = Sections[index];
            
            Console.SetCursorPosition(0, index);
            Console.Write(section);
        }
    }

    private void HandleInput()
    {
        var key = Console.ReadKey();
        
        switch (key.Key, key.Modifiers)
        {
            case (ConsoleKey.Escape, 0):
            {
                Exited = true;
                break;
            }
        }
    }
}