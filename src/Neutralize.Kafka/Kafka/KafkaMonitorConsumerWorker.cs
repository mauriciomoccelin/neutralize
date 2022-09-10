using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Kafka
{
    public class KafkaMonitorConsumerWorker : IHostedService
    {
        private Timer timer = null;
        private CancellationToken cancellationToken;

        private readonly ILogger<KafkaMonitorConsumerService> logger;
        private readonly IKafkaMonitorConsumerService kafkaMonitorConsumerService;

        public KafkaMonitorConsumerWorker(
            ILogger<KafkaMonitorConsumerService> logger,
            IKafkaMonitorConsumerService kafkaMonitorConsumerService
        )
        {
            this.logger = logger;
            this.kafkaMonitorConsumerService = kafkaMonitorConsumerService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.Zero);

            logger.LogInformation("Start kafka consumer service.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Dispose();
            kafkaMonitorConsumerService?.Dispose();

            logger.LogInformation("Stop kafka consumer service.");

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            while (true)
            {
                await kafkaMonitorConsumerService.Consume(cancellationToken);
            }
        }
    }
}