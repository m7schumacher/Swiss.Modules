using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Swiss.Web
{
    /// <summary>
    /// Utility class handles operation on HTML elements within web pages. Internal because of use of HtmlAgilityPack objects
    /// </summary>
    internal class HtmlUtility
    {
        /// <summary>
        /// Method recursively dives into HTML node tree in HTML document
        /// </summary>
        internal static List<HtmlNode> EnumerateNodeTree(HtmlNode root)
        {
            List<HtmlNode> localNodes = new List<HtmlNode>();
            localNodes.Add(root);

            foreach (var child in root.ChildNodes)
            {
                if (child.IsRelevant())
                {
                    localNodes.AddRange(EnumerateNodeTree(child));
                }
            }

            return localNodes;
        }

        /// <summary>
        /// Method wraps HtmlNode (from HtmlAgilityPack) into WebNode object
        /// </summary>
        internal static WebNode MakeWebNode(HtmlNode node, Uri url)
        {
            WebNode result = null;

            if (Tags.HtmlLink.EqualsIgnoreCase(node.Name))
            {
                result = new WebLink(node, url);
            }
            else if (Tags.HtmlTable.EqualsIgnoreCase(node.Name))
            {
                result = new WebTable(node);
            }
            else if (Tags.HtmlHead.EqualsIgnoreCase(node.Name))
            {
                result = new HtmlHeader(node);
            }
            else
            {
                result = new WebNode(node);
            }

            return result;
        }

        /// <summary>
        /// Method generates a List of WebNode wrappers by enumerating tree below a given HtmlNode root
        /// </summary>
        internal static List<WebNode> EnumeratePageTree(HtmlNode parent, Uri url)
        {
            WebNode root = MakeWebNode(parent, url);

            ConcurrentBag<WebNode> nodes = new ConcurrentBag<WebNode>();
            nodes.Add(root);

            foreach(var child in parent.ChildNodes)
            {
                var web = MakeWebNode(child, url);
                var locals = GenerateTree(web, url);
                locals.ForEach(nd => nodes.Add(nd));
            }

            return nodes.ToList();
        }

        /// <summary>
        /// Method generates a List of WebNode wrappers by enumerating tree below a given WebNode root
        /// </summary>
        internal static List<WebNode> GenerateTree(WebNode parent, Uri url)
        {
            List<WebNode> locals = new List<WebNode>();
            locals.Add(parent);

            foreach (var child in parent.Base.ChildNodes)
            {
                if (child.IsRelevant())
                {
                    var nd = MakeWebNode(child, url);
                    locals.AddRange(GenerateTree(nd, url));
                    parent.Children.Add(nd);
                }
            }

            return locals;
        }
    }
}
