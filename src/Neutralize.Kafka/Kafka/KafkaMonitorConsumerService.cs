using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Kafka
{
    public sealed class KafkaMonitorConsumerService : IKafkaMonitorConsumerService
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

        public void Dispose()
        {
            consumer?.Dispose();
        }

        public async Task Consume(CancellationToken cancellationToken)
        {
            if (!kafkaConfiguration.EnableMonitorHandler)
            {
                logger.LogWarning("Monitor service is disabled");
                return;
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                consumer ??= kafkaFactory.CreateConsumerForMonitor();
                consumer.Subscribe(kafkaConfiguration.Handlers.Keys.ToArray());

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
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
            }
        }
    }
}