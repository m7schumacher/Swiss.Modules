using ForecastIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiss.API
{
    public class Forecast : WeatherConditions
    {
        public DateTime Date { get; set; }
        public string DayOfWeek { get { return Date.DayOfWeek.ToString(); } }

        public int High { get; set; }
        public int Low { get; set; }

        public DateTime TimeOfHigh { get; set; }
        public DateTime TimeOfLow { get; set; }

        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }

        public Forecast(DailyForecast daily)
            : base(daily)
        {
            Date = daily.time.ToDate();
            High = (int)daily.temperatureMax.Round(0);
            Low = (int)daily.temperatureMin.Round(0);
            TimeOfHigh = daily.temperatureMaxTime.ToDate().ToLocalTime();
            TimeOfLow = daily.temperatureMinTime.ToDate().ToLocalTime();
            Sunrise = daily.sunriseTime.ToDate().ToLocalTime();
            Sunset = daily.sunsetTime.ToDate().ToLocalTime();
        }

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2} | {3}", DayOfWeek, High, Low, ChanceOfPrecip);
        }
    }
}
