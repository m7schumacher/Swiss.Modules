using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Device.Location;
using System.Timers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Swiss.API
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string FullAddress { get; set; }
    }

    public class LocationAPI
    {
        private static string MAP_URL = "http://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
        private static GeoCoordinateWatcher watcher;

        private static Address current;
        private static bool LocationFound;

        public static Address GetLocation()
        {
            watcher = new GeoCoordinateWatcher();

            LocationFound = false;
            UpdateLocation(null, null);

            while (!LocationFound) { }

            return current;
        }

        public static Address GetLocation(string ipAddress)
        {
            string baseURL = "http://freegeoip.net/xml/";
            var target = baseURL + ipAddress;

            var result = InternetUtility.DownloadXML(target);

            return new Address();
    }

        private static void UpdateLocation(object sender, ElapsedEventArgs args)
        {
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(GeoPositionChanged);
            watcher.Start();
        }

        private static void GeoPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var latitude = e.Position.Location.Latitude;
            var longitude = e.Position.Location.Longitude;

            MAP_URL = string.Format(MAP_URL, latitude, longitude);

            XElement sitemap = XElement.Load(MAP_URL);
            XElement results = sitemap.Elements().First(elem => elem.Name.LocalName.Equals("result"));
            XElement formatted_address = results.Elements().First(elem => elem.Name.LocalName.Equals("formatted_address"));

            current = ParseAddress(formatted_address.Value, latitude, longitude);

            watcher.Stop();
            LocationFound = true;
        }

        private static Address ParseAddress(string address, double lat, double lon)
        {
            string[] bits = address.Split(',');

            return new Address()
            {
                Street = bits[0],
                City = bits[1].Trim(),
                State = bits[2].Split(' ')[1],
                ZipCode = bits[2].Split(' ')[2],
                Country = bits[3].Trim(),
                Latitude = lat,
                Longitude = lon,
                FullAddress = address
            };
        }
    }
}
