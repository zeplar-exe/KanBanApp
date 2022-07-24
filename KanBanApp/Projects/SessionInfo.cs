using System.Xml.Linq;

namespace KanBanApp.Projects;

public class SessionInfo
{
    public ObjectPath OpenPath { get; set; }

    public static SessionInfo FromXml(Stream stream)
    {
        if (!stream.CanRead)
            throw new ArgumentException("Stream read is denied. Session deserialization aborted.");

        var document = XDocument.Load(stream);
        var info = new SessionInfo();

        if (document.Root == null)
            return info;
        
        var path = document.Root.Element("OpenPath")?.Value ?? "";

        info.OpenPath = ObjectPath.TryParse(path, out var openPath) ? openPath : new ObjectPath();
        
        return info;
    }

    public void WriteXml(Stream stream)
    {
        if (!stream.CanWrite)
            throw new ArgumentException("Stream write is denied. Session serialization aborted.");

        var root = new XElement("root");
        var document = new XDocument(root);
        
        root.Add(new XElement("OpenPath", OpenPath));
        
        document.Save(stream);
    }
}