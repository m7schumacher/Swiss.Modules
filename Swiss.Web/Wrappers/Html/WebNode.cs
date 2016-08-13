using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Swiss.Web
{
    //Could probably be a struct, but must be nullable for now
    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Wrapper class wraps around an HtmlNode from HtmlAgilityPack, extracting vital information and providing useful methods
    /// Also eliminates calling code from needing HtmlAgilityPack
    /// </summary>
    public class WebNode
    {
        //Note: Singleton pattern is followed for performance reasons - computationally intensive operations are only performed as needed
        internal HtmlNode Base { get; set; }

        private string _id;
        private string _text;
        private string _html;
        private string _type;
        private string _name;
        private string _xpath;

        public string ID { get { return _id ?? (_id = Base.Id); } }
        public string Text { get { return _text ?? (_text = Base.InnerText); } } 
        public string HTML { get { return _html ?? (_html = Base.OuterHtml); } }
        public string Type { get { return _type ?? (_type = Base.NodeType.ToString()); } }
        public string Name { get { return _name ?? (_name = Base.Name); } }

        public string XPath { get { return _xpath ?? (_xpath = Base.XPath); } }

        public List<WebNode> Children { get; set; }
        public List<Attribute> Attributes { get; set; }

        public bool IsRelevant { get { return !string.IsNullOrWhiteSpace(Text) && !IsParent; } }
        public bool IsParent { get { return Children.Count > 0; } }

        public WebNode(HtmlNode node)
        {
            Base = node;

            Children = new List<WebNode>();
            Attributes = node.Attributes.Select(attr => new Attribute()
            {
                Name = attr.Name,
                Value = attr.Value
            }).ToList();
        }

        public List<WebNode> ChildrenOfType(string type)
        {
            return Children.Where(ch => ch.Name.Equals(type)).ToList();
        }

        public string GetAttributeValue(string name)
        {
            var targetAttribute = Attributes.FirstOrDefault(attr => attr.Name.EqualsIgnoreCase(name));
            return targetAttribute != null ? targetAttribute.Value : "N/A";
        }

        public override bool Equals(object obj)
        {
            WebNode nd = obj as WebNode;
            return nd.Name.Equals(Name) && nd.Text.Equals(Text);
        }

        public bool IsMatch(Regex reg) { return reg.IsMatch(HTML); }
        public override int GetHashCode() { return Name.GetHashCode() + Text.GetHashCode(); }
        public override string ToString() { return String.Format("{0} --- '{1}'", Name, Text); }
    }

    /// <summary>
    /// Wrapper class extends WebNode to wrap an HTML link (<a> tag)
    /// </summary>
    public class WebLink : WebNode
    {
        private static Regex HrefPattern = new Regex(RegexPatterns.HrefHtml);
        public Uri Target;

        public WebLink(HtmlNode node, Uri url) : base(node)
        {
            var href = node.Attributes["href"];

            if(href != null)
            {
                var path = href.Value;
                Target = path.IsURL() ? new Uri(url, path) : null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} --- {1}", Text, Target.AbsoluteUri.ToString());
        }
    }

    /// <summary>
    /// Wrapper class extends WebNode to wrap an HTML Table, storing header and grid in string arrays for ease of use
    /// </summary>
    public class WebTable : WebNode
    {
        private static Regex TablePattern = new Regex(RegexPatterns.HrefHtml);
        public string[] Header { get; set; }
        public string[][] Grid { get; set; }

        public WebTable(HtmlNode node) : base(node)
        {
            var rows = node.Descendants("tr");

            if(rows.Count() > 0)
            {
                if (rows.First().Elements("th").Count() > 1)
                {
                    Header = rows.First().Elements("th").Select(th => th.InnerText.NormalizeWhiteSpace()).ToArray();
                }

                Grid = rows.Where(rw => rw.Elements("td").Count() > 1)
                           .Select(rw => rw.Elements("td").Select(td => td.InnerText.NormalizeWhiteSpace()).ToArray())
                           .ToArray();
            }
        }
    }
}
