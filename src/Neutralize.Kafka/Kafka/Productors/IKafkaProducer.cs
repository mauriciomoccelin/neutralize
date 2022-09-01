using System;
using System.Threading.Tasks;

namespace Neutralize.Kafka.Productors
{
    public interface IKafkaProducer : IDisposable
    {
        Task ProduceAsync(string topic, object value);
    }
}
