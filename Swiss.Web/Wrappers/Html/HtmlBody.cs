using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Swiss.Web
{
    /// <summary>
    /// Wrapper class wraps the HTML contents within the <body> tag of an HTML page, parsing Links and Tables as well
    /// </summary>
    public class HtmlBody
    {
        //Note: Singleton pattern is followed for performance reasons - computationally intensive operations are only performed as needed
        public List<WebNode> Nodes { get; private set; }

        private List<string> _innerTexts;
        private List<string> _innerHtmls;

        public List<string> InnerHTML { get { return _innerHtmls ?? (_innerHtmls = GetInnerHTML()); } }
        public List<string> InnerTexts { get { return _innerTexts ?? (_innerTexts = GetInnerTexts()); } }

        private List<WebTable> _tables;
        private List<WebLink> _links;

        public List<WebTable> Tables { get { return _tables ?? (_tables = Nodes.OfType<WebTable>().ToList()); } }
        public List<WebLink> Links { get { return _links ?? (_links = Nodes.OfType<WebLink>().Where(link => link.Target != null).ToList()); } }

        public HtmlBody(HtmlNode node, Uri url)
        {
            Nodes = HtmlUtility.EnumeratePageTree(node, url);
        }

        public List<string> GetMatches(Regex reg)
        {
            return InnerTexts.Where(txt => reg.IsMatch(txt)).ToList();
        }

        public string GetClosestMatch(string identifier)
        {
            return InnerTexts.GetClosestMatch(identifier).Key;
        }

        public WebNode GetNode(string id)
        {
            return Nodes.FirstOrDefault(nd => nd.ID.EqualsIgnoreCase(id));
        }

        public List<WebNode> GetNodesByType(string name)
        {
            return Nodes.Where(nd => nd.Name.EqualsIgnoreCase(name)).ToList();
        }

        private List<string> GetInnerTexts()
        {
            return Nodes.Select(nd => nd.Text)
                .Where(txt => !string.IsNullOrWhiteSpace(txt))
                .Where(txt => !Regex.IsMatch(txt, @"<.*>"))
                .Select(txt => HttpUtility.HtmlDecode(txt))
                .TrimAll()
                .ToList();
        }

        private List<string> GetInnerHTML()
        {
            return Nodes.Select(nd => nd.HTML)
                .Where(html => !html.IsEmpty())
                .ToList();
        }

        public override string ToString()
        {
            return "Body | " + Nodes.Count + " Nodes";
        }
    }
}
