using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class CurrentWeather : WeatherConditions
    {
        public int Temperature { get; set; }
        public int FeelsLike { get; set; }

        public int WindSpeed { get; set; }
        public WeatherEnums.Direction WindDirection { get; set; }

        public int Humidity { get; set; }
        public int DewPoint { get; set; }

        public int CloudCover { get; set; }
        public int ChanceOfPrecip { get; set; }

        public int DistanceNearestStorm { get; set; }
        public int Visibility { get; set; }

        public WeatherEnums.Direction DirectionNearestStorm { get; set; }

        public List<Forecast> Forecasts { get; set; }

        public Forecast Today { get { return Forecasts.First(); } }
        public Forecast Tomorrow { get { return Forecasts.ElementAt(1); } }

        public CurrentWeather(ForecastIOResponse response) : base(response.currently)
        {
            Forecasts = new List<Forecast>();

            var currentConditions = response.currently;

            Temperature = (int)currentConditions.temperature.Round(0);
            FeelsLike = (int)currentConditions.apparentTemperature.Round(0);

            DistanceNearestStorm = (int)currentConditions.nearestStormDistance.Round(0);
            DirectionNearestStorm = WeatherApiUtilities.CalculateDirection(currentConditions.nearestStormBearing);

            WindSpeed = currentConditions.windSpeed.Round(0).ToInt();
            WindDirection = WeatherApiUtilities.CalculateDirection(currentConditions.windBearing);

            Humidity = currentConditions.humidity.Round(0).ToInt();
            DewPoint = currentConditions.dewPoint.Round(0).ToInt();

            CloudCover = currentConditions.cloudCover.Round(0).ToInt();
            ChanceOfPrecip = currentConditions.precipProbability.Round(0).ToInt();

            Visibility = currentConditions.visibility.Round(0).ToInt();

            foreach (var daily in response.daily.data)
            {
                Forecasts.Add(new Forecast(daily));
            }
        }
    }
}
