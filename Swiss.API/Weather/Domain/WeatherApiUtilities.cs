using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class WeatherEnums
    {
        public enum Direction
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        };

        public enum PrecipType
        {
            Rain,
            Snow,
            Sleet,
            None
        }

        public enum PrecipIntensity
        {
            VeryLight,
            Light,
            Moderate,
            Heavy,
            None
        }
    }

    internal class WeatherApiUtilities
    {
        internal static WeatherEnums.Direction CalculateDirection(double degrees)
        {
            return (WeatherEnums.Direction)Math.Floor(degrees / 45);
        }

        internal static WeatherEnums.PrecipType CalculatePrecipType(string type)
        {
            switch (type)
            {
                case "rain": return WeatherEnums.PrecipType.Rain;
                case "snow": return WeatherEnums.PrecipType.Snow;
                case "sleet": return WeatherEnums.PrecipType.Sleet;
                default: return WeatherEnums.PrecipType.None;
            }
        }

        internal static WeatherEnums.PrecipIntensity CalculatePrecipIntensity(float intensity)
        {
            var result = intensity > .4 ? WeatherEnums.PrecipIntensity.Heavy :
                   intensity > .1 ? WeatherEnums.PrecipIntensity.Moderate :
                   intensity > .016 ? WeatherEnums.PrecipIntensity.Light :
                   intensity > 0 ? WeatherEnums.PrecipIntensity.VeryLight :
                   WeatherEnums.PrecipIntensity.None;

            return result;
        }

        internal static double MakePercentage(float value)
        {
            return (value * 100).ToDouble().Round(0);
        }
    }
}
