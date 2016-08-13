using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class Excerpt
    {
        public string Book { get; set; }
        public string Chapter { get; set; }
        public string Number { get; set; }
        public string Verse { get; set; }
    }

    public class Bible
    {
        private static string url = "http://labs.bible.org/api/?passage=";

        public static Excerpt GetVerses(string verses)
        {
            string searchURL = url + verses + "&type=json";

            WebClient client = new WebClient();
            string json = client.DownloadString(searchURL).TrimCharactersWithin(new char[] { '[', ']' });

            string book = string.Empty;
            string chapter = string.Empty;
            string number = null;
            string verse = string.Empty;

            string[] splitter = json.Split('{');

            for (int i = 1; i < splitter.Length; i++)
            {
                int end = i == splitter.Length - 1 ? splitter[i].Length : splitter[i].Length - 1;
                string srch = "{" + splitter[i].Substring(0, end);
                JToken token = JObject.Parse(srch);

                if (i == splitter.Length - 1 && splitter.Length > 2)
                {
                    number += " through " + token.SelectToken("verse").ToString();
                }
                else
                {
                    number = number ?? token.SelectToken("verse").ToString();
                    chapter = token.SelectToken("chapter").ToString();
                    book = token.SelectToken("bookname").ToString();
                }

                verse += token.SelectToken("text").ToString() + " ";
            }

            return new Excerpt()
            {
                Book = book,
                Number = number,
                Chapter = chapter,
                Verse = verse
            };
        }
    }
}
