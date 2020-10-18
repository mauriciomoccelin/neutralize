using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Neutralize.Kafka.Consumers
{
    public class KafkaMonitorConsumerService : BackgroundService
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
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Kafka consumer is starting.");

            await Task.Factory.StartNew(Start, stoppingToken);
        }

        private async Task Start()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(configuration.ConsumerConfig).Build())
            {
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };
                consumer.Subscribe(configuration.Handlers.Keys);

                try
                {
                    while (!cts.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);
                            var type = configuration.Handlers.First(k => k.Key.Equals(consumeResult.Topic));
                            var result = JsonConvert.DeserializeObject(consumeResult.Message.Value, type.Value);
                            if (result is null)
                                Console.WriteLine("The tópic {0} for offset {1} is null", consumeResult.Topic,
                                    consumeResult.TopicPartitionOffset);
                            else await mediator.Publish(result, cts.Token);
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

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Kafka consumer is stoping.");

            await base.StopAsync(stoppingToken);
        }
    }
}