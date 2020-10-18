using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;

namespace Neutralize.Tests
{
    public sealed class WeatherForecastHandler : INotificationHandler<WeatherForecast>
    {
        public Task Handle(WeatherForecast notification, CancellationToken cancellationToken)
        {
            notification.Should().NotBeNull();
            notification.TemperatureF.Should().Be(78);
            
            return Task.CompletedTask;
        }
    }
}