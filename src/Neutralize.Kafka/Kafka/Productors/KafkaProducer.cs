using System.Threading.Tasks;
using Confluent.Kafka;
using Neutralize.Kafka.Helpers;
using Neutralize.Kafka.Models;

namespace Neutralize.Kafka.Productors
{
    public sealed class KafkaProducer : BaseProducer, IKafkaProducer
    {
        private readonly IReportDelivery reportDelivery;

        public KafkaProducer(IKafkaConfiguration configuration, IReportDelivery reportDelivery) : base(configuration)
        {
            this.reportDelivery = reportDelivery;
        }

        public async Task ProduceAsync(string topic, object value)
        {
            await ProducerWrapper(
                producer => producer.Produce(topic, value.ToMessage(), DeliveryHandler)
            );
        }

        private void DeliveryHandler(DeliveryReport<Null, string> report)
        {
            if (reportDelivery is null || report is null) return;

            if (report.Error.IsError && ReportDeliveryOnFailure())
            {
                reportDelivery.OnFailure(
                    DeliveryFailedModel.Create(
                        report.Error.Reason, report.Topic, report.Message.Value
                    )
                ).Wait();
            }
            else if (ReportDeliveryOnSuccess())
            {
                reportDelivery.OnSuccess(
                    DeliverySuccessModel.Create(
                        report.Topic, report.Offset.ToString(), report.Message.Value,
                        report.TopicPartitionOffset.Partition.Value.ToString()
                    )
                ).Wait();
            }
        }

        private bool ReportDeliveryOnFailure() => !string.IsNullOrWhiteSpace(Configuration.TopicFailureDelivery);
        private bool ReportDeliveryOnSuccess() => !string.IsNullOrWhiteSpace(Configuration.TopicSuccessDelivery);
    }
}
