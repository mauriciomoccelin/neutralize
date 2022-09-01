namespace Neutralize.Kafka.Models
{
    public class DeliverySuccessModel
    {
        public string Topic { get; }
        public string Offset { get; }
        public string Message { get; }
        public string PartitionOffset { get; }

        private DeliverySuccessModel(string topic, string offset, string message, string partitionOffset)
        {
            Topic = topic;
            Offset = offset;
            Message = message;
            PartitionOffset = partitionOffset;
        }

        public static DeliverySuccessModel Create(string topic, string offset, string message, string partitionOffset)
        {
            return  new DeliverySuccessModel(topic, offset, message, partitionOffset);
        }
    }
}
