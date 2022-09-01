using System;
using System.Threading.Tasks;
using Neutralize.Kafka.Models;

namespace Neutralize.Kafka.Productors
{
    public interface IReportDelivery : IDisposable
    {
        Task OnFailure(DeliveryFailedModel model);
        Task OnSuccess(DeliverySuccessModel model);
    }
}
