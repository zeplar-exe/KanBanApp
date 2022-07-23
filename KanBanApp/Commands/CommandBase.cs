namespace KanBanApp.Commands;

public abstract class CommandBase
{
    [Option("-q|--quiet", CommandOptionType.NoValue)]
    public bool Quiet { get; set; }
    
    [Option("--exit-code", CommandOptionType.NoValue)]
    public bool ExitCode { get; set; }

    protected virtual int OnExecute()
    {
        int code;
        
        try
        {
            code = Execute();
        }
        catch (Exception e)
        {
            code = e.HResult;
            
            WriteOutput(e.Message);
        }
        
        if (ExitCode)
            WriteOutputLine(code.ToString());

        return code;
    }
    
    protected abstract int Execute();

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