namespace KanBanApp.Commands;

public abstract class CommandBase
{
    [Option("-q|--quiet", CommandOptionType.NoValue)]
    public bool Quiet { get; set; }
    
    [Option("--exit-code", CommandOptionType.NoValue)]
    public bool ExitCode { get; set; }

    protected abstract int OnExecute();

    protected int Exit(int code)
    {
        if (ExitCode)
            WriteOutputLine(code.ToString());
        
        return code;
    }

    protected void WriteException(Exception exception)
    {
        WriteOutputLine(exception.Message);
    }

    protected void WriteOutputLine(string output)
    {
        WriteOutput(output + Environment.NewLine);
    }
    
    protected void WriteOutput(string output)
    {
        if (Quiet)
            return;
        
        Console.Write(output);
    }
}