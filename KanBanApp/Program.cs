namespace KanBanApp;

public static class Program
{
    public static int Main(string[] args)
    {
        return CommandLineApplication.Execute<Entry>(args);
    }
}