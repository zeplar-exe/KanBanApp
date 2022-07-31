using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using KanBanApp.DefaultConsole;

namespace KanBanApp.Commands;

public class Edit : CommandBase
{
    [Argument(0, "Path")]
    [Required]
    public string Path { get; set; }

    [Option("--reserve-buffer", CommandOptionType.NoValue)]
    public bool ReserveBuffer { get; set; }

    protected override int Execute()
    {
        var sections = new[] { "Name", "Description" };
        var editor = new CursesEditor(sections);

        using var reserved = new MemoryStream();

        if (ReserveBuffer)
        {
            using var reservedWriter = new StreamWriter(reserved, leaveOpen: true);
            
            var width = (short)Console.BufferWidth;
            var height = (short)Console.BufferHeight;
            
            reservedWriter.Write(ConsoleReader.ReadFromBuffer(0, 0, width, height));
            reservedWriter.Flush();
        }

        editor.Run();

        reserved.Position = 0;
        using var reservedReader = new StreamReader(reserved);
        
        Console.Write(reservedReader.ReadToEnd());
        
        return 0;
    }
}