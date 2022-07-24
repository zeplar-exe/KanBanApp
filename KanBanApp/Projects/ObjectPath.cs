using System.Text;

namespace KanBanApp.Projects;

public readonly struct ObjectPath
{
    public string Board { get; }
    public string List { get; }
    public string Card { get; }
    
    public ObjectPath(string board = "", string list = "", string card = "")
    {
        Board = board;
        List = list;
        Card = card;
    }

    public static bool TryParse(string path, out ObjectPath objectPath)
    {
        objectPath = default;
        
        var split = path.Split('/');

        if (split.Length > 3)
            return false;

        objectPath = new ObjectPath(
            split.ElementAtOrDefault(0) ?? "", 
            split.ElementAtOrDefault(1) ?? "", 
            split.ElementAtOrDefault(2) ?? "");

        return true;
    }

    public override int GetHashCode()
    {
        return 61 * Board.GetHashCode() + List.GetHashCode() + Card.GetHashCode();
    } // See https://stackoverflow.com/a/16824761/16324801

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append(Board);

        if (!string.IsNullOrWhiteSpace(List))
        {
            builder.Append('/').Append(List);
        }
        
        if (!string.IsNullOrWhiteSpace(Card))
        {
            builder.Append('/').Append(Card);
        }

        return builder.ToString();
    }
}