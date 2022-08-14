using Xunit;
using FluentAssertions;
using System.Linq;
using Microsoft.Extensions.Hosting;
using MediatR;

namespace Neutralize.Kafka.Test;

[Collection(nameof(KafkaFixtureCollection))]
public class KafkaAbstraction_Test
{
    private readonly KafkaFixture fixture;

    public KafkaAbstraction_Test(KafkaFixture fixture)
    {
        this.fixture = fixture;
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When register kafka for producer and consumer only must be registered")]
    public void AddKafka_Factory_MustBeSuccess()
    {
        // Arranges
        var services = fixture.GenereteServiceCollection();

        // Act
        services.AddKafka(options => options.SetFlushTimeout(byte.MaxValue));

        // Assert
        services.Count.Should().Be(2);
        services.Any(s => s.ServiceType == typeof(IKafkaFactory)).Should().BeTrue();
        services.Any(s => s.ServiceType == typeof(IKafkaConfiguration)).Should().BeTrue();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When register kafka for producer, consumer and monitor consumer handlers")]
    public void AddKafka_FactoryAndMonitor_MustBeSuccess()
    {
        // Arranges
        var services = fixture.GenereteServiceCollection();

        // Act
        services.AddKafka(
            options => options
                .AddHandler("test", typeof(Notification_Fake)),
            typeof(KafkaAbstraction_Test).Assembly
        );

        // Assert
        services.Count.Should().Be(11);
        services.Any(s => s.ServiceType == typeof(IKafkaFactory)).Should().BeTrue();
        services.Any(s => s.ServiceType == typeof(IKafkaConfiguration)).Should().BeTrue();
        services.Any(s => s.ServiceType == typeof(IMediator)).Should().BeTrue();
        services.Any(s => s.ServiceType == typeof(IHostedService)).Should().BeTrue();
    }
}