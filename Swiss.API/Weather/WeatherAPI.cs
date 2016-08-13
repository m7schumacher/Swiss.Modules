using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiss;

namespace Swiss.API
{
    public class WeatherConditions
    {
        public int WindSpeed { get; set; }
        public WeatherEnums.Direction WindDirection { get; set; }

        public int Humidity { get; set; }
        public int Visibility { get; set; }

        public int Cloudiness { get; set; }
        public int DewPoint { get; set; }
        
        public int ChanceOfPrecip { get; set; }
        public WeatherEnums.PrecipType TypeOfPrecip { get; set; }
        public WeatherEnums.PrecipIntensity IntesityOfPrecip { get; set; }

        public string Summary { get; set; }

        public WeatherConditions(Currently currentConditions)
        {
            WindSpeed = (int)currentConditions.windSpeed.Round(0);
            WindDirection = WeatherApiUtilities.CalculateDirection(currentConditions.windBearing);
            Visibility = (int)currentConditions.visibility.Round(0);
            Humidity = (int)WeatherApiUtilities.MakePercentage(currentConditions.humidity);
            Cloudiness = (int)WeatherApiUtilities.MakePercentage(currentConditions.cloudCover);
            DewPoint = (int)currentConditions.dewPoint.Round(0);
            
            ChanceOfPrecip = (int)WeatherApiUtilities.MakePercentage(currentConditions.precipProbability);
            TypeOfPrecip = WeatherApiUtilities.CalculatePrecipType(currentConditions.precipType);
            IntesityOfPrecip = WeatherApiUtilities.CalculatePrecipIntensity(currentConditions.precipIntensity);

            Summary = currentConditions.summary;
        }

        public WeatherConditions(DailyForecast daily)
        {
            WindSpeed = (int)daily.windSpeed.Round(0);
            WindDirection = WeatherApiUtilities.CalculateDirection(daily.windBearing);
            Visibility = (int)daily.visibility.Round(0);
            Humidity = (int)WeatherApiUtilities.MakePercentage(daily.humidity);
            Cloudiness = (int)WeatherApiUtilities.MakePercentage(daily.cloudCover);
            DewPoint = (int)daily.dewPoint.Round(0);

            ChanceOfPrecip = (int)WeatherApiUtilities.MakePercentage(daily.precipProbability);
            TypeOfPrecip = WeatherApiUtilities.CalculatePrecipType(daily.precipType);
            IntesityOfPrecip = WeatherApiUtilities.CalculatePrecipIntensity(daily.precipIntensity);

            Summary = daily.summary;
        }
    }

    public class WeatherAPI
    {
        private string Key { get; set; }

        public WeatherAPI(string key)
        {
            Key = key;
        }

        public CurrentWeather GetCurrentWeather(double lat, double lon)
        {
            var request = new ForecastIORequest(Key, (float)lat, (float)lon, Unit.us);
            var response = request.Get();

            return new CurrentWeather(response);
        }
    }
}
