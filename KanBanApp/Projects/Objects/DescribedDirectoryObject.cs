using KanBanApp.Helpers;

namespace KanBanApp.Projects.Objects;

public abstract class DescribedDirectoryObject : DescribedObject
{
    public new static IEnumerable<TObject> FromContent<TObject>(string contentDirectory, ContentLoader loader, string elementName) 
        where TObject : DescribedDirectoryObject, new()
    {
        foreach (var item in loader.Load<TObject>(elementName))
        {
            var directory = Path.Join(contentDirectory, item.Name);
            FileSystemHelper.EnsureDirectory(directory);

            item.FromDirectory(directory);

            yield return item;
        }
    }

    public abstract void FromDirectory(string path);
    public abstract void WriteDirectory(string path);
}