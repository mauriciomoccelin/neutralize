using System;
using Neutralize.Kafka.Productors;
using Xunit;

namespace Neutralize.Tests
{
    public class Producer_Test : BaseKafkaTest
    {
        private readonly IKafkaProducer producer;
        public Producer_Test()
        {
            producer = Resolve<IKafkaProducer>();
        }
        
        [Fact]
        public async void Test()
        {
            var forecast = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Scorching",
                TemperatureC = 26
            };
            
            await producer.ProduceAsync("forecast", forecast);
        }

        public override void Dispose()
        {
            producer?.Dispose();
        }
    }
}