using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Swiss
{
    /// <summary>
    /// Wrapper class wraps an XElement object with various helpful properties and methods
    /// </summary>
    public class XmlNode
    {
        private XElement _node { get; set; }

        public List<XmlNode> Children { get; private set; }

        public bool IsLeaf { get { return Children.Count == 0; } }
        public bool IsCollection { get { return CheckIfCollection(); } }

        public string Name { get; set; }
        public string File { get; set; }

        public int LineNumber { get { return ((IXmlLineInfo)_node).LineNumber; } }
        public int LinePosition { get { return ((IXmlLineInfo)_node).LinePosition; } }

        public XmlNodeType Type { get; private set; }

        public Dictionary<string, string> Attributes { get; set; }

        public string Value { get; set; }

        public XmlNode(XElement elem)
        {
            _node = elem;

            Children = elem.Elements()
                           .Select(nd => new XmlNode(nd))
                           .ToList();

            Attributes = elem.Attributes()
                             .Select(attr => new KeyValuePair<string, string>(attr.Name.ToString(), attr.Value.ToString()))
                             .ToDictionary();

            Name = _node.Name.LocalName;
            File = _node.BaseUri;
            Value = _node.Value;
            Type = _node.NodeType;

            var attributes = _node.Attributes();
        }

        public string GetAttribute(string name)
        {
            return Attributes.ContainsKey(name) ? Attributes[name] : null;
        }

        public XmlNode GetChildByName(string nameOfChild)
        {
            return Children.FirstOrDefault(child => child.Name.Equals(nameOfChild));
        }

        public bool HasChild(string name)
        {
            return Children.Any(ch => ch.Name.EqualsIgnoreCase(name));
        }

        public void AddAttribute(string name, string value)
        {
            Attributes.Add(name, value);
        }

        private bool CheckIfCollection()
        {
            bool hasAttributes = _node.HasAttributes;
            bool hasChildren = _node.HasElements;

            if (!hasAttributes && hasChildren)
            {
                var elements = _node.Elements();
                var distinctElements = elements.GetDistinctValuesOfField(nd => nd.Name.ToString());

                if (distinctElements.Count() == 1 && elements.Count() > 1)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + ":");
            builder.AppendEach(Attributes.Select(attr => attr.Key + ":"));

            return builder.ToString();
        }
    }
}
