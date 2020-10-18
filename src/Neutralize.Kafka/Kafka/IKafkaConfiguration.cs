using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace Neutralize.Kafka
{
    public interface IKafkaConfiguration
    {
        TimeSpan FlushTimeout { get; }
        string TopicFailureDelivery { get; }
        string TopicSuccessDelivery { get; }
        ProducerConfig ProducerConfig { get; }
        ConsumerConfig ConsumerConfig { get; }
        public IDictionary<string, Type> Handlers { get; }
        
        void AddHandler(string topic, Type handlers);
    }
}
