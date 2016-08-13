//using Abot.Crawler;
//using Abot.Poco;
//using System;
//using System.Collections.Generic;

//namespace Swiss.Web
//{
//    public class CrawlResult
//    {
//        public List<string> TotalURLs { get; set; }
//        public List<string> GoodURLs { get; set; }
//        public List<string> BadURLs { get; set; }

//        public List<WebPage> Pages { get; set; }

//        public CrawlResult()
//        {
//            TotalURLs = new List<string>();
//            GoodURLs = new List<string>();
//            BadURLs = new List<string>();

//            Pages = new List<WebPage>();
//        }
//    }

//    /// <summary>
//    /// Utility class built on top of Abot web crawler for quickly crawling and scraping an entire website
//    /// </summary>
//    public class WebCrawler
//    {
//        private CrawlResult _result { get; set; }

//        public bool IsForcedLinkParsingEnabled { get; set; }
//        public bool IsRespectAnchorRelNoFollowEnabled { get; set; }
//        public bool IsRespectHttpXRobotsTagHeaderNoFollowEnabled { get; set; }
//        public bool IsRespectMetaRobotsNoFollowEnabled { get; set; }
//        public bool IsRespectRobotsDotTextEnabled { get; set; }
//        public int MaxConcurrentThreads { get; set; }
//        public int MaxCrawlDepth { get; set; }
//        public int MaxPagesToCrawl { get; set; }

//        public WebCrawler()
//        {
//            MaxPagesToCrawl = int.MaxValue;
//            MaxCrawlDepth = int.MaxValue;
//            MaxConcurrentThreads = 64;
//            IsForcedLinkParsingEnabled = true;
//            IsRespectAnchorRelNoFollowEnabled = false;
//            IsRespectMetaRobotsNoFollowEnabled = false;
//            IsRespectHttpXRobotsTagHeaderNoFollowEnabled = false;
//            IsRespectRobotsDotTextEnabled = false;
//        }

//        private void Refresh()
//        {
//            _result = new CrawlResult();
//        }

//        /// <summary>
//        /// Method crawls site, enumerates URLs, and can be set to scrape the pages it finds as well
//        /// </summary>
//        public CrawlResult CrawlSite(string rootURL, bool onlyURLs = true)
//        {
//            return CrawlSite(new Uri(rootURL), onlyURLs);
//        }

//        /// <summary>
//        /// Method crawls site, enumerates URLs, and can be set to scrape the pages it finds as well
//        /// </summary>
//        public CrawlResult CrawlSite(Uri rootURL, bool onlyURLs = true)
//        {
//            Refresh();

//            var config = new CrawlConfiguration
//            {
//                MaxPagesToCrawl = this.MaxPagesToCrawl,
//                MaxCrawlDepth = this.MaxCrawlDepth,
//                MaxConcurrentThreads = this.MaxConcurrentThreads,
//                IsForcedLinkParsingEnabled = this.IsForcedLinkParsingEnabled,
//                IsRespectAnchorRelNoFollowEnabled = this.IsRespectAnchorRelNoFollowEnabled,
//                IsRespectMetaRobotsNoFollowEnabled = this.IsRespectMetaRobotsNoFollowEnabled,
//                IsRespectHttpXRobotsTagHeaderNoFollowEnabled = this.IsRespectHttpXRobotsTagHeaderNoFollowEnabled,
//                IsRespectRobotsDotTextEnabled = this.IsRespectRobotsDotTextEnabled,
//            };

//            using (var crawler = new PoliteWebCrawler(config))
//            {
//                if (onlyURLs) crawler.PageCrawlCompleted += PageCrawl;
//                else crawler.PageCrawlCompleted += PageCrawlAndScrape;

//                crawler.Crawl(rootURL);
//            }

//            return _result;
//        }

//        private void AddToResult(string url, bool isGoodPage)
//        {
//            _result.TotalURLs.Add(url);

//            if (isGoodPage) _result.GoodURLs.Add(url);
//            else _result.BadURLs.Add(url);
//        }

//        private void PageCrawlAndScrape(object sender, PageCrawlCompletedArgs e)
//        {
//            var url = e.CrawledPage.Uri.ToString();
//            var isGoodPage = e.CrawledPage.WebException == null;

//            AddToResult(url, isGoodPage);

//            if(isGoodPage)
//            {
//                var page = WebScraper.ScrapePage(url);
//                _result.Pages.Add(page);
//            }
//        }

//        private void PageCrawl(object sender, PageCrawlCompletedArgs e)
//        {
//            var url = e.CrawledPage.Uri.ToString();
//            var isGoodPage = e.CrawledPage.WebException == null;

//            AddToResult(url, isGoodPage);
//        }
//    }
//}
