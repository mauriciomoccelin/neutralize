using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Kafka;

namespace Neutralize.Tests
{
    public class BaseKafkaTest: NeutralizeBaseTest
    {
        protected BaseKafkaTest()
        {
            services
                .AddKafkaAssemblyHandlers(Assembly.Load("Neutralize.Kafka.Test"))
                .AddKafka(configuration, options => options.AddHandler("forecast", typeof(WeatherForecast)));
            
            provider = services.BuildServiceProvider();
        }
        
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}