using MediatR;

namespace Neutralize.Kafka
{
    public interface IIKafkaConsumerHandler<in TNotification>
        : INotificationHandler<TNotification> where TNotification : INotification
    {
    }
}