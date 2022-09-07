using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Kafka
{
    public class KafkaMonitorConsumerService : IHostedService
    {
        private readonly IMediator mediator;
        private readonly IKafkaFactory kafkaFactory;
        private readonly IKafkaConfiguration kafkaConfiguration;
        private readonly ILogger<KafkaMonitorConsumerService> logger;
        private IConsumer<Ignore, string> consumer;

        public KafkaMonitorConsumerService(
            IMediator mediator,
            IKafkaFactory kafkaFactory,
            IKafkaConfiguration kafkaConfiguration,
            ILogger<KafkaMonitorConsumerService> logger
        )
        {
            this.mediator = mediator;
            this.kafkaFactory = kafkaFactory;
            this.kafkaConfiguration = kafkaConfiguration;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            consumer = kafkaFactory.CreateConsumerForMonitor();
            consumer.Subscribe(kafkaConfiguration.Handlers.Keys.ToArray());

            logger.LogInformation("Starting kafka monitor consumer service.");

            do
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    var type = kafkaConfiguration.Handlers.First(k => k.Key.Equals(consumeResult.Topic));
                    var result = JsonConvert.DeserializeObject(consumeResult.Message.Value, type.Value);
                    if (result is INotification)
                    {
                        logger.LogInformation(
                            "Received message: tópic ({0}), offset ({1}), partition ({2})",
                            consumeResult.Topic,
                            consumeResult.Offset,
                            consumeResult.Partition.Value
                        );

                        await mediator.Publish(result, cancellationToken);
                    }
                    else
                    {
                        logger.LogWarning(
                            "The tópic {0} for offset {1} is null",
                            consumeResult.Topic,
                            consumeResult.TopicPartitionOffset
                        );
                    }
                }
                catch (ConsumeException exception)
                {
                    logger.LogError(exception, exception.Error.Reason);
                }
            }
            while (!cancellationToken.IsCancellationRequested && kafkaConfiguration.EnableMonitorHandler);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer?.Close();
            logger.LogInformation("Stop kafka consumer service.");

            return Task.CompletedTask;
        }
    }
}