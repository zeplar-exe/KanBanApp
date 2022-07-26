using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using KanBanApp.Common;
using KanBanApp.Projects;
using KanBanApp.Projects.Objects;

namespace KanBanApp.Commands;

public class Add : CommandBase
{
    [Argument(0, "Path")]
    [Required]
    public string Path { get; set; }

    [Option("-f|--force")]
    public bool Force { get; set; }
    
    protected override int Execute()
    {
        if (!ObjectPath.TryParse(Path, out var path))
        {
            WriteOutputLine($"The path '{Path}' is invalid (bad format).");
            
            return 1;
        }

        if (path.IsRoot())
        {
            WriteOutputLine("Cannot add a root path.");

            return 1;
        }

        AssertProjectExists(out var project);

        if (!TryAddBoard(path, project, out var board))
            return 1;

        if (!string.IsNullOrEmpty(path.List))
        {
            if (!TryAddList(path, project, board))
                return 1;
        }

        WriteOutputLine($"Created '{path.ToString()}'");
        
        project.Write();

        return 0;
    }

    private bool TryAddBoard(ObjectPath path, ProjectInterface project, [NotNullWhen(true)] out ProjectBoard? board)
    {
        board = null;
        
        var boardHash = StringHash.Hash(path.Board).ToString();
        var existingBoard = project.Boards.FirstOrDefault(b => b.Id == boardHash);

        if (existingBoard != null)
        {
            if (path.IsBoard())
            {
                if (Force)
                {
                    project.Boards.Remove(existingBoard);
                }
                else
                {
                    WriteOutputLine($"The path '{path.ToString()}' already exists. Use --force to overwrite.");

                    return false;
                }
            }
        }
        else
        {
            if (!path.IsBoard())
            {
                if (!project.Configuration.AutoCreatePathParts)
                {
                    WriteOutputLine($"The board '{path.Board}' does not exist.");

                    return false;
                }
            }
        }

        board = new ProjectBoard { Name = path.Board };
        project.Boards.Add(board);

        return true;
    }

    private bool TryAddList(ObjectPath path, ProjectInterface project, ProjectBoard board)
    {
        var listHash = StringHash.Hash(path.List).ToString();
        var existingList = board.Lists.FirstOrDefault(l => l.Id == listHash);

        if (existingList != null)
        {
            if (path.IsList())
            {
                if (Force)
                {
                    board.Lists.Remove(existingList);
                }
                else
                {
                    WriteOutputLine($"The list '{path.ToString()}' already exists. Use --force to overwrite.");

                    return false;
                }
            }
        }
        else
        {
            if (!path.IsList())
            {
                if (!project.Configuration.AutoCreatePathParts)
                {
                    WriteOutputLine($"The list '{path.List}' does not exist.");

                    return false;
                }
            }
        }

        var list = new BoardList { Name = path.List };
        board.Lists.Add(list);
        
        if (!string.IsNullOrEmpty(path.Card))
        {
            if (!TryAddCard(path, list))
                return false;
        }

        return true;
    }

    private bool TryAddCard(ObjectPath path, BoardList list)
    {
        var cardHash = StringHash.Hash(path.Card).ToString();
        var existingCard = list.Cards.FirstOrDefault(l => l.Id == cardHash);

        if (existingCard != null)
        {
            if (path.IsCard())
            {
                if (Force)
                {
                    list.Cards.Remove(existingCard);
                }
                else
                {
                    WriteOutputLine($"The card '{path.ToString()}' already exists. Use --force to overwrite.");

                    return false;
                }
            }
        }

        var card = new ListCard { Name = path.Card };
        list.Cards.Add(card);

        return true;
    }
}