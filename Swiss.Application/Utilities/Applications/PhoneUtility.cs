using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.Utilities.Applications
{
    public class PhoneUtility
    {
        public static string SendSMS(string fromNumber, string toNumber, string smsBody, string apiToken)
        {
            String URI = "http://www.puretext.us" +
              "/service/sms/send?" +
                "fromNumber=" + fromNumber +
                "&toNumber=" + toNumber +
                "&smsBody=" + System.Net.WebUtility.UrlEncode(smsBody) +
                "&apiToken=" + apiToken;

            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return "404:URL not found :" + URI;
                            break;
                        case HttpStatusCode.BadRequest:
                            return "400:Bad Request";
                            break;

                        default:
                            return httpWebResponse.StatusCode.ToString();
                    }
                }
            }
            return null;
        }
    }
}
