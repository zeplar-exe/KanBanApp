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
        
        var path = document.Root.Element("openPath")?.Value ?? "";

        info.OpenPath = ObjectPath.TryParse(path, out var openPath) ? openPath : new ObjectPath();
        
        return info;
    }

    public void WriteXml(Stream stream)
    {
        if (!stream.CanWrite)
            throw new ArgumentException("Stream write is denied. Session serialization aborted.");

        var document = new XDocument(new XElement("root"));
        
        document.Root.Add(new XElement("openPath", OpenPath));
        
        document.Save(stream);
    }
}