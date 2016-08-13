using System;
using System.Collections.Generic;

namespace Swiss.Web
{
    /// <summary>
    /// Wrapper class wraps contents of HTML page
    /// </summary>
    public class WebPage
    {
        public Uri URL { get; private set; }

        public HtmlHeader Head { get; set; }
        public HtmlBody Body { get; set; }

        public List<WebNode> Nodes { get { return Body.Nodes; } }

        internal WebPage(HtmlHeader header, HtmlBody body)
        {
            Head = header;
            Body = body;
        }
    }
}
