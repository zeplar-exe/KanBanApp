namespace KanBanApp.Extensions;

public static class ListExtensions
{
    public static void CopyTo<T>(this List<T> list, List<T> target)
    {
        target.AddRange(list);
    }
}