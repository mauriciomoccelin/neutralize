using Confluent.Kafka;

namespace Neutralize.Kafka;

public interface IKafkaFactory
{
    IProducer<TKey, TValue> CreateProducer<TKey, TValue>();
    IConsumer<TKey, TValue> CreateConsumer<TKey, TValue>();
}
