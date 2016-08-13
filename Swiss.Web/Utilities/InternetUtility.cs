using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Swiss.Web
{
    public class PingResult
    {
        public List<string> GoodPages { get; set; }
        public List<string> BadPages { get; set; }

        public PingResult()
        {
            GoodPages = new List<string>();
            BadPages = new List<string>();
        }
    }

    /// <summary>
    /// Utility class contains methods to ping pages, download web content, and other related functions
    /// </summary>
    public static class InternetUtility
    {
        /// <summary>
        /// Method pings a list of URLs in parallel and returns a PingResult containing good and bad pages
        /// </summary>
        public static PingResult PingMany(IEnumerable<string> urls)
        {
            ConcurrentBag<string> valid = new ConcurrentBag<string>();
            ConcurrentBag<string> invalid = new ConcurrentBag<string>();

            Parallel.ForEach(urls, new ParallelOptions { MaxDegreeOfParallelism = 10 }, url =>
            {
                if (PingPage(url)) valid.Add(url);
                else invalid.Add(url);
            });

            return new PingResult()
            {
                GoodPages = valid.ToList(),
                BadPages = invalid.ToList()
            };
        }

        /// <summary>
        /// Method makes a WGet request by opening up a CMD process in the background and downloading content from URL to specified location
        /// </summary>
        public static void MakeWGetRequest(string url, string targetFolder, bool downloadEverything = true)
        {
            string args = (downloadEverything ? "--page-requisites " : string.Empty) + url;

            Process p = new Process();
            p.StartInfo.WorkingDirectory = targetFolder;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = @"wget.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = args;
            p.Start();
        }

        /// <summary>
        /// Method makes a get request and returns, in string format, the data returned
        /// </summary>
        public static string MakeWebRequest(string url, int timeout = 1000)
        {
            string result = string.Empty;
            string line = string.Empty;

            WebRequest wrGETURL = WebRequest.Create(url);
            wrGETURL.Timeout = timeout;

            try
            {
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                while (line != null)
                {
                    line = objReader.ReadLine();
                    result += line + "\n";
                }

                return result;
            }
            catch(Exception trouble)
            {
                return trouble.Message;
            }
        }

        /// <summary>
        /// Method downloads JSON from URL, serializes it into an XML format, and wrap in an XMLSheet wrapper
        /// </summary>
        public static XmlSheet DownloadJSON(string url)
        {
            string json;

            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(url);
            }

            XDocument doc = JsonConvert.DeserializeXmlNode(json, "root").ToXDocument();
            return new XmlSheet(doc);
        }

        /// <summary>
        /// Method downloads XML from URL, serializes and return a wrapped XMLSheet - Can handle JSON calls as well
        /// </summary>
        public static XmlSheet DownloadXML(string url)
        {
            string xml;

            using (var webClient = new WebClient())
            {
                xml = webClient.DownloadString(url);
            }

            XDocument doc;

            try
            {
                doc = XDocument.Parse(xml);
            }
            catch(Exception trouble)
            {
                doc = JsonConvert.DeserializeXmlNode(xml).ToXDocument();
            }

            return new XmlSheet(doc);
        }

        /// <summary>
        /// Method uses RestSharp to ping page and returns true or false depending on result
        /// </summary>
        public static bool PingPage(string url)
        {
            try
            {
                var client = new RestClient(url);
                var response = client.Get(new RestRequest());

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Method checks if machine is connected to the internet
        /// </summary>
        public static bool CheckForInternetConnection()
        {
            try
            {
                //Target URL returns formatted text, the result of which can be tested
                string targetURL = "http://sports.espn.go.com/mlb/bottomline/scores";

                WebRequest request = WebRequest.Create(targetURL);
                WebResponse response = request.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string read = reader.ReadToEnd();

                bool connected = read.Contains("&mlb_s_delay");

                return connected;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
