﻿namespace KanBanApp.Commands;

[Command("init")]
public class Init : CommandBase
{
    [Option("-d|--dir <DirectoryPath>", CommandOptionType.SingleValue)]
    public string? DirectoryPath { get; set; }

    protected override int OnExecute()
    {
        var target = PathResolver.RelativeOrAbsoluteFilePath(DirectoryPath);
        var board = new ProjectInterface(target);
        
        board.Write();
        
        WriteOutputLine($"Initialized a kba project in '{target}'.");

        return Exit(0);
    }
}