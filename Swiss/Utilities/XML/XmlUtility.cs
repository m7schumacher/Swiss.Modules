using System.Xml.Linq;

namespace Swiss
{
    public class XmlUtility
    {
        public static XmlSheet GenerateSheet(string file)
        {
            XDocument doc = XDocument.Load(file);
            return new XmlSheet(doc);
        }
    }
}
