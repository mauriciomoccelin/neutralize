using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace Neutralize.Kafka
{
    public interface IKafkaConfiguration
    {
        TimeSpan FlushTimeout { get; }
        bool EnableMonitorHandler { get; }
        ConsumerConfig ConsumerConfig { get; }
        ProducerConfig ProducerConfig { get; }
        public IDictionary<string, Type> Handlers { get; }

        void SetFlushTimeout(byte flushTimeout);
        void EnableMonitor(bool enable = true);
        void SetConsumerConfig(string group, string bootstrapServers);
        void SetProducerConfig(string bootstrapServers);
        void AddHandler(string topic, Type handlers);
    }
}
