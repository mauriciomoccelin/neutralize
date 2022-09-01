using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Kafka.Consumers
{
    public class KafkaMonitorConsumerService : IHostedService
    {
        private readonly IMediator mediator;
        private readonly IKafkaConfiguration configuration;

        public KafkaMonitorConsumerService(
            IMediator mediator,
            IKafkaConfiguration configuration
        )
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kafka consumer is starting.");
            Task.Factory.StartNew(StartHandlerDelegate, cancellationToken);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine("Kafka consumer is stoping.");
            return Task.CompletedTask;
        }
        
        private async Task StartHandlerDelegate()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(configuration.ConsumerConfig).Build())
            {
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };
                consumer.Subscribe(configuration.Handlers.Keys.ToArray());

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);
                            var type = configuration.Handlers.First(k => k.Key.Equals(consumeResult.Topic));
                            var result = JsonConvert.DeserializeObject(consumeResult.Message.Value, type.Value);
                            if (result is INotification)
                            {
                                Console.WriteLine(
                                    "Received message: tópic ({0}), offset ({1}), partition ({2})",
                                    consumeResult.Topic, consumeResult.Offset, consumeResult.Partition.Value
                                );
                                await mediator.Publish(result, cts.Token);
                            }
                            else
                                Console.WriteLine(
                                    "The tópic {0} for offset {1} is null",
                                    consumeResult.Topic, consumeResult.TopicPartitionOffset
                                );
                        }
                        catch (ConsumeException exception)
                        {
                            Console.WriteLine(exception.Error.Reason);
                        }
                    }
                }
                catch (OperationCanceledException exception)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(exception.Message);
                    Console.ResetColor();
                    consumer.Close();
                }
            }
        }
    }
}