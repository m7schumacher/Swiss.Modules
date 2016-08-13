using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class AlchemyApiCall
    {
        private string Base { get { return "http://gateway-a.watsonplatform.net/calls/text/"; } }
        internal string Key { get; set; }

        public string Text { get; set; }
        public string Function { get; set; }

        public string Call { get { return string.Format("{0}{1}?apikey={2}&text={3}&showSourceText=1&outputMode=json", Base, Function, Key, Text); } }
    }

    public class LanguageAPI
    {
        private AlchemyApiCall Caller { get; set; }

        public LanguageAPI(string key)
        {
            Caller = new AlchemyApiCall()
            {
                Key = key
            };
        }

        private static string Filter(string input)
        {
            string output = input.Trim()
                               .TrimPunctuation()
                               .ToLower();

            return output;
        }

        public Dictionary<string, double> GetKeyWords(string sentence)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Caller.Function = "TextGetRankedKeywords";
            Caller.Text = Filter(sentence);

            var call = Caller.Call;
            XmlSheet sheet = InternetUtility.DownloadJSON(call);

            var keywords = sheet.NodesByName("keywords");

            foreach(var keyword in keywords)
            {
                var text = keyword.GetChildByName("text").Value;
                var conf = Convert.ToDouble(keyword.GetChildByName("relevance").Value).Round(2);

                result.Add(text, conf);
            }

            return result;
        }

        public Dictionary<string, double> GetTaxonomy(string sentence)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Caller.Function = "TextGetRankedTaxonomy";
            Caller.Text = Filter(sentence);

            var call = Caller.Call;
            XmlSheet sheet = InternetUtility.DownloadJSON(call);

            var taxons = sheet.NodesByName("taxonomy");

            foreach (var taxon in taxons.Where(tx => !tx.HasChild("confident")))
            {
                var text = taxon.GetChildByName("label").Value;
                var conf = Convert.ToDouble(taxon.GetChildByName("score").Value).Round(2);

                result.Add(text, conf);
            }

            return result;
        }

        public Dictionary<string, double> GetConcepts(string sentence)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            Caller.Function = "TextGetRankedConcepts";
            Caller.Text = Filter(sentence);

            var call = Caller.Call;
            XmlSheet sheet = InternetUtility.DownloadJSON(call);

            var concepts = sheet.NodesByName("concepts");

            foreach (var concept in concepts)
            {
                var text = concept.GetChildByName("text").Value;
                var conf = Convert.ToDouble(concept.GetChildByName("relevance").Value).Round(2);

                result.Add(text, conf);
            }

            return result;
        }
    }
}
