using System.Threading.Tasks;
using Neutralize.Kafka.Helpers;
using Neutralize.Kafka.Models;

namespace Neutralize.Kafka.Productors
{
    public sealed class ReportDelivery : BaseProducer, IReportDelivery
    {
        public ReportDelivery(IKafkaConfiguration configuration) : base(configuration)
        {
        }

        public async Task OnFailure(DeliveryFailedModel model)
        {
            await ProducerWrapper(
                async producer => await producer.ProduceAsync(
                    Configuration.TopicFailureDelivery, model.ToMessage()
                )
            );
        }

        public async Task OnSuccess(DeliverySuccessModel model)
        {
            await ProducerWrapper(
                async producer => await producer.ProduceAsync(
                    Configuration.TopicSuccessDelivery, model.ToMessage()
                )
           );
        }
    }
}
