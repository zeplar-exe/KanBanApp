using System.Text;

namespace KanBanApp.Projects;

public readonly record struct ObjectPath(string Board = "", string List = "", string Card = "")
{
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

    public ObjectPath AsBoard() => new(Board);
    public ObjectPath AsList() => new(Board, List);
    public ObjectPath AsCard() => new(Board, List, Card);

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