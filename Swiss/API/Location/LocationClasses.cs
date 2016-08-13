using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class GeographicCoordinates
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public GeographicCoordinates()
        {
            Latitude = string.Empty;
            Longitude = string.Empty;
        }
    }

    public class Location
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public GeographicCoordinates LatAndLong { get; set; }
        public DateTime LocalTime { get; set; }

        public Location()
        {

        }

        public Location(GeographicCoordinates latlon, TimeZoneInfo timezone)
        {
            LatAndLong = latlon;
            LocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, timezone.Id);
        }

        public Location(string city, string state, string zip, GeographicCoordinates coordinates, TimeZoneInfo timezone)
            : this(coordinates, timezone)
        {
            City = city;
            State = state;
            ZipCode = zip;
        }
    }
}
