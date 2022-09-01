using MediatR;

namespace Neutralize.Kafka.Consumers
{
    public interface IIKafkaConsumerHandler<in TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
    }
}