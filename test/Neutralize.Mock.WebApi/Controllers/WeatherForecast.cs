using System;
using MediatR;

namespace Neutralize.Tests.Controllers
{
    public class WeatherForecast : INotification
    {
        public DateTime Date { get; set; }
        public string Summary { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public override string ToString() => $"{Date} | {Summary} | C°: {TemperatureC} | F°: {TemperatureF}";
    }
}
