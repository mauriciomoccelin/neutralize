using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Neutralize.Kafka.Productors
{
    public abstract class BaseProducer : IDisposable
    {
        protected readonly IKafkaConfiguration Configuration;

        protected BaseProducer(IKafkaConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected async Task ProducerWrapper(Action<IProducer<Null, string>> producer)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    using (var producerBuild = new ProducerBuilder<Null, string>(Configuration.ProducerConfig).Build())
                    {
                        producer.Invoke(producerBuild);
                        producerBuild.Flush(Configuration.FlushTimeout);
                    }
                });
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("KAFKA: Delivery Error:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(exception.Message);
            }
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
