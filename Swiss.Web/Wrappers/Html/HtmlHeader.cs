using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace Swiss.Web
{
    public class MetaData
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public override string ToString() { return string.Format("{0} | {1}", Name ?? "N/A", Content ?? "N/A"); }
    }

    public class Script
    {
        public string Type { get; set; }
        public string Source { get; set; }
        public string Code { get; set; }

        public bool IsJavaScript { get { return Type.Equals("text/javascript") || Source.EndsWith(".js"); } }

        public override string ToString() { return string.Format("{0} | {1}", Type ?? "N/A", Source ?? "N/A"); }
    }

    public class CssFile
    {
        public string Path { get; set; }
        public string Media { get; set; }

        public override string ToString() { return string.Format("{0} | {1}", Path ?? "N/A", Media ?? "N/A"); }
    }

    /// <summary>
    /// Wrapper class wraps the contents within the <head> tag of an HTML document, gathering scripts, files and other metadata
    /// </summary>
    public class HtmlHeader : WebNode
    {
        //Note: Singleton pattern is followed for performance reasons - computationally intensive operations are only performed as needed
        private List<MetaData> _metas { get; set; }
        public List<Script> _scripts { get; set; }
        public List<CssFile> _styleSheets { get; set; }

        public List<MetaData> Metas { get { return _metas ?? (_metas = GatherMetas()); } }
        public List<Script> Scripts { get { return _scripts ?? (_scripts = GatherScripts()); } }
        public List<CssFile> StyleSheets { get { return _styleSheets ?? (_styleSheets = GatherStyleSheets()); } }

        public HtmlHeader(HtmlNode node) : base(node) { }

        private List<MetaData> GatherMetas()
        {
            var metas = Children.Where(nd => nd.Name.Equals("meta")).ToList();

            return metas.Select(meta => new MetaData()
            {
                Name = meta.GetAttributeValue("name"),
                Content = meta.GetAttributeValue("content")
            }).ToList();
        }

        private List<Script> GatherScripts()
        {
            var scripts = Children.Where(nd => nd.Name.Equals("script")).ToList();

            return scripts.Select(script => new Script()
            {
                Type = script.GetAttributeValue("type"),
                Source = script.GetAttributeValue("src"),
                Code = script.Text
            }).ToList();
        }

        private List<CssFile> GatherStyleSheets()
        {
            var styles = Children.Where(nd => nd.Name.Equals("link"))
                                      .Where(lnk => lnk.GetAttributeValue("rel").Equals("stylesheet"))
                                      .ToList();

            return styles.Select(style => new CssFile()
            {
                Path = style.GetAttributeValue("href"),
                Media = style.GetAttributeValue("media")
            }).ToList();
        }
    }
}
