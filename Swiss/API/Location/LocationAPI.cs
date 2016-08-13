using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class LocationAPI : API
    {
        private string GeoCoordinatesURL = "http://maps.googleapis.com/maps/api/geocode/xml?address=";
        private string CityStateURL = "http://ziptasticapi.com/";
        private string TimeZoneURL = "https://maps.googleapis.com/maps/api/timezone/json?location={0},{1}&timestamp=1458000000";

        public GeographicCoordinates GetGeographicCoordinates(string zipcode)
        {
            var result = SafelyMakeAPICall(() =>
            {
                string call = GeoCoordinatesURL + zipcode;
                var data = InternetUtility.DownloadXML(call);

                var loc = new GeographicCoordinates()
                {
                    Latitude = data.Leaves.First(lv => lv.Name.Equals("lat")).Value,
                    Longitude = data.Leaves.First(lv => lv.Name.Equals("lng")).Value
                };

                return loc;
            });

            return result;
        }

        public Location GetLocation(string zip)
        {
            GeographicCoordinates coordinates = GetGeographicCoordinates(zip);

            var cityAndState = GetCityAndState(zip);
            string stateAbbreviation = cityAndState.Item1;
            string cityName = cityAndState.Item2;

            TimeZoneInfo timeZone = GetTimeZone(coordinates);

            return new Location(cityName, stateAbbreviation, zip, coordinates, timeZone);
        }

        public Tuple<string, string> GetCityAndState(string zip)
        {
            var result = SafelyMakeAPICall(() =>
            {
                string fullURI = CityStateURL + zip;

                var jsonAsString = InternetUtility.MakeWebRequest(fullURI);
                var jsonObject = (JObject)JsonConvert.DeserializeObject(jsonAsString);

                return new Tuple<string, string>(jsonObject["state"].ToString(), jsonObject["city"].ToString());
            });

            return result;
        }

        public TimeZoneInfo GetTimeZone(GeographicCoordinates latAndLong)
        {
            var lat = latAndLong.Latitude;
            var lon = latAndLong.Longitude;

            string baseURI = string.Format(TimeZoneURL,lat, lon);

            var result = SafelyMakeAPICall(() =>
            {
                var data = InternetUtility.MakeWebRequest(baseURI);
                var lines = data.Split("\n");
                var line = lines[5].Split(":")[1].Remove("\"").Trim();

                string timeZoneID = line.SplitOnWhiteSpace()[0].Trim() + " Standard Time";

                TimeZoneInfo info = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);

                return info;
            });

            return result;
        }
    }
}
