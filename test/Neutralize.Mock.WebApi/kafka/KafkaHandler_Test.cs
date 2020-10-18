using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Neutralize.Tests.Controllers;

namespace Neutralize.Tests.kafka
{
    public class KafkaHandler_Test : INotificationHandler<WeatherForecast>
    {
        public Task Handle(WeatherForecast notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification);
            return Task.CompletedTask;
        }
    }
}