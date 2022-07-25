namespace KanBanApp.Extensions;

public static class DictionaryExtensions
{
    public static void CopyTo<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TValue> target)
    {
        foreach (var pair in dictionary)
        {
            target[pair.Key] = pair.Value;
        }
    }
}