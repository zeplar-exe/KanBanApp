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
            ForceWriteOutputLine(code.ToString());

        return code;
    }
    
    protected abstract int Execute();

    protected void AssertProjectExists(out ProjectInterface project)
    {
        project = new ProjectInterface(Directory.GetCurrentDirectory());
        
        if (!ProjectInterface.ExistsIn(Directory.GetCurrentDirectory()))
        {
            throw new ProjectMissingException(Directory.GetCurrentDirectory());
        }
        
        project.Update();
    }

    protected void WriteOutputLine(string output)
    {
        WriteOutput(output + Environment.NewLine);
    }
    
    protected void ForceWriteOutputLine(string output)
    {
        ForceWriteOutput(output + Environment.NewLine);
    }
    
    protected void WriteOutput(string output)
    {
        if (Quiet)
            return;
        
        Console.Write(output);
    }
    
    protected void ForceWriteOutput(string output)
    {
        Console.Write(output);
    }
}

public class ProjectMissingException : Exception
{
    public ProjectMissingException(string directory) : base($"A kba project does not exist in '{directory}'.")
    {
        HResult = 1;
    }
}