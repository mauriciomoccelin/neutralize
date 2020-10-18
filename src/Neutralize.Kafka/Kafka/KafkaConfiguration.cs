using System;
using System.Collections.Generic;
using Confluent.Kafka;

namespace Neutralize.Kafka
{
    public sealed class KafkaConfiguration : IKafkaConfiguration
    {
        public string Group { get; }
        public TimeSpan FlushTimeout { get; }
        public string TopicFailureDelivery { get; }
        public string TopicSuccessDelivery { get; }
        public ProducerConfig ProducerConfig { get; }
        
        public IDictionary<string, Type> Handlers { get; }
        
        public ConsumerConfig ConsumerConfig { get; }

        private KafkaConfiguration(
            string group,
            byte flushSeconds,
            string bootstrapServers, 
            string topicFailureDelivery, 
            string topicSuccessDelivery
        )
        {
            Group = group;
            FlushTimeout = TimeSpan.FromSeconds(flushSeconds);
            TopicFailureDelivery = topicFailureDelivery;
            TopicSuccessDelivery = topicSuccessDelivery;
            ProducerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };
            ConsumerConfig = new ConsumerConfig
            { 
                GroupId = "forecast",
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            Handlers = new Dictionary<string, Type>();
        }

        public void AddHandler(string topic, Type handlers)
        {
            Handlers.Add(new KeyValuePair<string, Type>(topic, handlers));
        }
        
        public static KafkaConfiguration Create(
            string group,
            byte flushSeconds,
            string bootstrapServers,
            string topicFailureDelivery,
            string topicSuccessDelivery
        )
        {
            return new KafkaConfiguration(
                group, flushSeconds, bootstrapServers, topicFailureDelivery, topicSuccessDelivery
            );
        }
    }
}
