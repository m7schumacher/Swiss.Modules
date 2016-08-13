using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Swiss.Web
{
    /// <summary>
    /// Utility class scrapes HTML of web pages and wraps them in a WebPage wrapper class
    /// </summary>
    public class WebScraper
    {
        #region Page Scraping - Only One Page

        /// <summary>
        /// Method scrapes only links from an HTML page
        /// </summary>
        public static List<WebLink> ScrapeLinks(string url)
        {
            Uri head = new Uri(url);
            HtmlDocument doc = url.Download();

            List<HtmlNode> nds = HtmlUtility.EnumerateNodeTree(doc.DocumentNode);

            return nds.Where(nd => nd.Name.EqualsIgnoreCase(Tags.HtmlLink))
                            .Select(nd => new WebLink(nd, head))
                            .ToList();
        }

        /// <summary>
        /// Method scrapes only tables from an HTML page
        /// </summary>
        public static List<WebTable> ScrapeTables(string url)
        {
            Uri head = new Uri(url);
            HtmlDocument doc = url.Download();

            List<HtmlNode> nds = HtmlUtility.EnumerateNodeTree(doc.DocumentNode);

            return nds.Where(nd => nd.Name.EqualsIgnoreCase(Tags.HtmlTable))
                            .Select(nd => new WebTable(nd))
                            .ToList();
        }

        /// <summary>
        /// Method scrapes entire specified html page and wraps in WebPage wrapper
        /// </summary>
        public static WebPage ScrapePage(string url, bool thurough = true)
        {
            Uri uri = new Uri(url);
            HtmlDocument doc = url.Download();

            return ScrapePage(doc, uri);
        }

        private static WebPage ScrapePage(HtmlDocument doc, Uri uri)
        {
            var rt = doc.DocumentNode;
            var html = rt.ChildNodes["html"];
            var hd = html.ChildNodes["head"];
            var bd = html.ChildNodes["body"];

            HtmlHeader header = new HtmlHeader(hd);
            HtmlBody body = new HtmlBody(bd, uri);

            var nodes = HtmlUtility.EnumerateNodeTree(rt).Where(nd => nd.Name.Equals("table")).ToList();

            return new WebPage(header, body);
        }

        #endregion


        #region Site Crawling - More Than One Page

        /// <summary>
        /// Method crawls and scrapes entire website, wrapping in a WebSit wrapper
        /// </summary>
        //public static WebSite ScrapeSite(string rootURL, string name = "", int pagesToCrawl = int.MaxValue)
        //{
        //    return ScrapeSite(new Uri(rootURL), name, pagesToCrawl);
        //}

        ///// <summary>
        ///// Method crawls and scrapes entire website, wrapping in a WebSit wrapper
        ///// </summary>
        //public static WebSite ScrapeSite(Uri rootURL, string name = "", int pagesToCrawl = int.MaxValue)
        //{
        //    WebCrawler crawler = new WebCrawler();
        //    crawler.MaxPagesToCrawl = pagesToCrawl;

        //    var crawlResult = crawler.CrawlSite(rootURL);
        //    var nameOfSite = string.IsNullOrEmpty(name) ? rootURL.Host.ToString() : name;

        //    return new WebSite(nameOfSite, crawlResult.Pages);
        //}

        #endregion
    }
}
