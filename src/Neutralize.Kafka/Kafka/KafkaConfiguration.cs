using MediatR;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neutralize.Kafka
{
    public sealed class KafkaConfiguration : IKafkaConfiguration
    {
        public TimeSpan FlushTimeout { get; private set; }
        public bool EnableMonitorHandler { get; private set; }
        public ConsumerConfig ConsumerConfig { get; private set; }
        public ProducerConfig ProducerConfig { get; private set; }
        public IDictionary<string, Type> Handlers { get; }

        private KafkaConfiguration()
        {
            Handlers = new Dictionary<string, Type>();
        }

        public void SetFlushTimeout(byte flushTimeout)
        {
            if (flushTimeout <= 0)
            {
                throw new ArgumentException("Flush timeout must be greater than zero", nameof(flushTimeout));
            }

            FlushTimeout = TimeSpan.FromSeconds(flushTimeout);
        }

        public void EnableMonitor(bool enable = true)
        {
            EnableMonitorHandler = enable;
        }

        public void SetConsumerConfig(string group, string bootstrapServers)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                throw new ArgumentException("Group cannot be null or empty", nameof(group));
            }

            if (string.IsNullOrWhiteSpace(bootstrapServers))
            {
                throw new ArgumentException("BootstrapServers cannot be null or empty", nameof(bootstrapServers));
            }

            ConsumerConfig = new ConsumerConfig
            {
                GroupId = group,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public void SetProducerConfig(string bootstrapServers)
        {
            if (string.IsNullOrWhiteSpace(bootstrapServers))
            {
                throw new ArgumentException("BootstrapServers cannot be null or empty", nameof(bootstrapServers));
            }

            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };
        }

        public void AddHandler(string topic, Type handlers)
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentException("Topic cannot be null or empty", nameof(topic));
            }

            if (!handlers.GetInterfaces().Contains(typeof(INotification)))
            {
                throw new ArgumentException($"{handlers.Name} is not assignable from {typeof(INotification).Name}");
            }

            Handlers.Add(new KeyValuePair<string, Type>(topic, handlers));
        }

        public static KafkaConfiguration Create()
        {
            return new KafkaConfiguration();
        }
    }
}
