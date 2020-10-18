using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neutralize.Kafka.Productors;

namespace Neutralize.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", 
            "Bracing", 
            "Chilly", 
            "Cool", 
            "Mild", 
            "Warm", 
            "Balmy", 
            "Hot", 
            "Sweltering", 
            "Scorching"
        };

        private readonly IKafkaProducer _producer;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IKafkaProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
        
        [HttpPost]
        public IEnumerable<WeatherForecast> Post()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPost]
        [Route("with-params")]
        public WeatherForecast Post([FromBody] WeatherForecast forecast)
        {
            _producer.ProduceAsync("forecast", forecast);
            return forecast;
        }
        
        [HttpPut]
        public IEnumerable<WeatherForecast> Put()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPut]
        [Route("with-params")]
        public WeatherForecast Put([FromBody] WeatherForecast forecast)
        {
            return forecast;
        }
        
        [HttpDelete]
        public Task Delete()
        {
            var rng = new Random();
            return Task.CompletedTask;
        }
    }
}
