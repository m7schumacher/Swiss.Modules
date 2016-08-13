using System.Collections.Generic;

namespace Swiss.Web
{
    /// <summary>
    /// Wrapper class wraps a list of WebPage objects. Has very little functionality right now, but could be built upon
    /// </summary>
    public class WebSite
    {
        public List<WebPage> Pages { get; set; }
        public string Name { get; set; }

        public WebSite(string name, List<WebPage> pages)
        {
            Pages = pages;
            Name = name;
        }

        public void AddPage(WebPage page)
        {
            Pages.Add(page);
        }
    }
}
