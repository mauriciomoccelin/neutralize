namespace Neutralize.Kafka.Models
{
    public sealed class DeliveryFailedModel
    {
        public string Error { get; }
        public string Topic { get; }
        public string Message { get; }

        private DeliveryFailedModel(string error, string topic, string message)
        {
            Error = error;
            Topic = topic;
            Message = message;
        }

        public static DeliveryFailedModel Create(string error, string topic, string message)
            => new DeliveryFailedModel(error, topic, message);
    }
}
