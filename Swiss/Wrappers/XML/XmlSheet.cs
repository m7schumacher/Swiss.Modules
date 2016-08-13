using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Swiss
{
    /// <summary>
    /// Wrapper class wraps XDocument object with helper properties and methods
    /// </summary>
    public class XmlSheet
    {
        public XDocument Document { get; set; }

        public List<XmlNode> Nodes { get; private set; }
        public List<XmlNode> Leaves { get { return Nodes.Where(nd => nd.IsLeaf).ToList(); } }//nodes that have no children
        public List<XmlNode> Branches { get { return Nodes.Where(nd => !nd.IsLeaf).ToList(); } }//nodes that do have children

        public XmlNode Root { get; private set; }

        public XmlSheet(XDocument doc)
        {
            Document = doc;

            Root = new XmlNode(Document.Root);
            Nodes = GatherNodes(Root);
        }

        public List<XmlNode> NodesByName(string name)
        {
            return Nodes.Where(nd => nd.Name.Equals(name)).ToList();
        }

        public List<XmlNode> NodesByString(string str)
        {
            return Nodes.Where(nd => nd.ToString().Equals(str)).ToList();
        }

        private List<XmlNode> GatherNodes(XmlNode parent)
        {
            List<XmlNode> nodes = new List<XmlNode>();
            nodes.Add(parent);

            foreach(var child in parent.Children)
            {
                nodes.AddRange(GatherNodes(child));
            }

            return nodes;
        }
    }
}
