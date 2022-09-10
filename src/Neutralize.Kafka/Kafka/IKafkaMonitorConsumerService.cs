using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Kafka
{
    public interface IKafkaMonitorConsumerService : IDisposable
    {
        Task Consume(CancellationToken cancelationToken);
    }
}