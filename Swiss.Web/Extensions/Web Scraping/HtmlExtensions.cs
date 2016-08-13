using HtmlAgilityPack;
using System.Linq;

namespace Swiss.Web
{
    /// <summary>
    /// Internal extensions class for extending funcionality of HtmlAgilityPack objects
    /// </summary>
    internal static class HtmlExtensions
    {
        /// <summary>
        /// Method determines whether this HtmlNode is relevant (has text or is a parent, and isn't a comment)
        /// </summary>
        internal static bool IsRelevant(this HtmlNode node)
        {
            bool hasText = !string.IsNullOrWhiteSpace(node.InnerText);
            bool isParent = node.ChildNodes.Where(nd => nd.IsRelevant()).Count() > 0;
            bool isComment = node.NodeType == HtmlNodeType.Comment;

            bool hasContent = hasText || isParent;

            return hasText && !isComment;
        }

        /// <summary>
        /// Method treats this string as a URL and downloads it
        /// </summary>
        internal static HtmlDocument Download(this string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
    }
}
