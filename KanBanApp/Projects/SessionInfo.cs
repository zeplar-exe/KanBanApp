using System.Xml.Linq;

namespace KanBanApp.Projects;

public class SessionInfo
{
    public ObjectPath OpenPath { get; set; }

    public static SessionInfo FromXml(XDocument document)
    {
        var info = new SessionInfo();

        if (document.Root == null)
            return info;
        
        var path = document.Root.Element("openPath")?.Value ?? "";

        info.OpenPath = ObjectPath.TryParse(path, out var openPath) ? openPath : new ObjectPath();
        
        return info;
    }

    public void WriteXml(XElement root)
    {
        root.Add(new XElement("openPath", OpenPath));
    }
}