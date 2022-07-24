using KanBanApp.Projects;

namespace KanBanApp.Commands;

public abstract class CommandBase
{
    [Option("-q|--quiet", CommandOptionType.NoValue)]
    public bool Quiet { get; set; }
    
    [Option("--exit-code", CommandOptionType.NoValue)]
    public bool ExitCode { get; set; }
    
    [Option("-v|--verbose", CommandOptionType.NoValue)]
    public bool Verbose { get; set; }

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
            
            WriteOutputLine(Verbose ? e.ToString() : e.Message);
        }
        
        if (ExitCode)
            WriteOutputLine(code.ToString());

        return code;
    }
    
    protected abstract int Execute();

    protected bool TestProjectExists(out ProjectInterface project)
    {
        project = new ProjectInterface(Directory.GetCurrentDirectory());
        
        if (!ProjectInterface.ExistsIn(Directory.GetCurrentDirectory()))
        {
            WriteOutputLine($"A kba project does not exist in '{Directory.GetCurrentDirectory()}'.");
        }
        
        project.Update();

        return true;
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