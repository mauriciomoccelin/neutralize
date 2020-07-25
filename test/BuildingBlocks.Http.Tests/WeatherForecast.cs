using System;

namespace BuildingBlocks.Http.Tests
{
    public class WeatherForecast
    {
        public DateTime Date { get; private set; }
        public int TemperatureC { get; private set; }
        public int TemperatureF { get; private set; }
        public string Summary { get; private set; }

        public WeatherForecast() { }

        public WeatherForecast(
            DateTime date, 
            int temperatureC, 
            int temperatureF, 
            string summary
        )
        {
            Date = date;
            TemperatureC = temperatureC;
            TemperatureF = temperatureF;
            Summary = summary;
        }
    }
}