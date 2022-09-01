using Confluent.Kafka;

namespace Neutralize.Kafka;

public class KafkaFactory : IKafkaFactory
{
    private readonly IKafkaConfiguration Configuration;

    public KafkaFactory(IKafkaConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IProducer<TKey, TValue> CreateProducer<TKey, TValue>()
    {
        var kafkaJsonSerializer = new KafkaJsonSerializer<TValue>();
        var producerBuilder = new ProducerBuilder<TKey, TValue>(Configuration.ProducerConfig);

        producerBuilder.SetValueSerializer(kafkaJsonSerializer);

        return producerBuilder.Build();
    }

    public IConsumer<TKey, TValue> CreateConsumer<TKey, TValue>()
    {
        var deserializer = new KafkaJsonDeserializer<TValue>();
        var consumerBuilder = new ConsumerBuilder<TKey, TValue>(Configuration.ConsumerConfig);

        consumerBuilder.SetValueDeserializer(deserializer);

        return consumerBuilder.Build();
    }
}