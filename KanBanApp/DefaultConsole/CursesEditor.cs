namespace KanBanApp.DefaultConsole;

public class CursesEditor
{
    private const int TextFieldHeight = 10;

    public const string SectionSeparator = "\fNEW_SECTION\f";

    private static string TextFieldBarrier = string.Join("", Enumerable.Repeat('$', Console.BufferWidth));
    
    private List<SectionTextField> Fields { get; }
    private bool Exited { get; set; }

    public CursesEditor(IEnumerable<string> sections)
    {
        Fields = sections.Select(s => new SectionTextField { Header = s }).ToList();
    }
    
    public string Run()
    {
        try
        {
            Console.Clear();
            
            while (!Exited)
            {
                Render();
                HandleInput();
            }
        }
        finally
        {
            Console.Clear();
            
            Fields.Clear();
            Exited = false;
        }

        return string.Join(SectionSeparator, Fields.Select(f => f.FullText));
    }

    private void Render()
    {
        Console.Clear();

        var line = 0;

        void MoveLine()
        {
            line++;
            MoveCursorToLine();
        }

        void MoveCursorToLine()
        {
            Console.SetCursorPosition(0, line);
        }
        
        MoveCursorToLine();
        
        foreach (var field in Fields)
        {
            WriteColor(field.Header, ConsoleColor.White, ConsoleColor.Black);
            MoveLine();
            
            if (field.LineOffset < TextFieldHeight)
            {
                Console.Write(TextFieldBarrier);
                MoveLine();
            }
            
            Console.Write(field.FullText);
            
            MoveLine();
            
            if (line > field.LineCount - TextFieldHeight)
            {
                Console.Write(TextFieldBarrier);
                MoveLine();
            }
        }
    }

    private void WriteColor(string text, ConsoleColor background, ConsoleColor foreground)
    {
        var oldBackground = Console.BackgroundColor;
        var oldForeground = Console.ForegroundColor;

        Console.BackgroundColor = background;
        Console.ForegroundColor = foreground;

        Console.Write(text);
        
        Console.BackgroundColor = oldBackground;
        Console.ForegroundColor = oldForeground;
    }

    private void HandleInput()
    {
        var key = Console.ReadKey();
        
        switch (key.Key, key.Modifiers)
        {
            case (ConsoleKey.Q, ConsoleModifiers.Control):
            {
                Exited = true;
                break;
            }
        }
    }

    private class SectionTextField
    {
        public string Header { get; set; } = "Unnamed Field";
        
        public string FullText { get; set; } = "";
        public int LineOffset { get; set; } = 0;

        public int LineCount
        {
            get
            {
                var count = 0;
                var index = 0;

                foreach (var line in SplitLines(FullText))
                {
                    index = 0;
                    count++;

                    foreach (var c in line)
                    {
                        index++;

                        if (++index >= Console.BufferWidth)
                        {
                            index = 0;
                            count++;
                        }
                    }
                }

                return count;
            }
        }

        public void Write()
        {
            
        }
        
        private static string[] SplitLines(string source) {
            return source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
    }
}