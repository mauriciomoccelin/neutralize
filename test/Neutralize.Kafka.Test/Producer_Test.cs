using Microsoft.Extensions.DependencyInjection;
using Neutralize.Kafka.Productors;
using Neutralize.Tests;
using Xunit;

namespace Neutralize.Kafka.Test
{
    public class Producer_Test : NeutralizeBaseTest
    {
        private readonly IKafkaProducer producer;
        public Producer_Test()
        {
            services.AddKafka(configuration);
            provider = services.BuildServiceProvider();
            
            producer = Resolve<IKafkaProducer>();
        }
        
        [Fact]
        public void Test()
        {
            producer.ProduceAsync("test", "Hello Worls!");
        }

        public override void Dispose()
        {
            producer?.Dispose();
        }
    }
}