using Xunit;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Moq;
using FluentAssertions;
using System;

namespace Neutralize.Kafka.Test;

[Collection(nameof(KafkaFixtureCollection))]
public class KafkaMonitorConsumerService_Test
{
    private readonly KafkaFixture fixture;

    public KafkaMonitorConsumerService_Test(KafkaFixture fixture)
    {
        this.fixture = fixture;
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When start async, consume event as notification")]
    public async Task StartAsync_ConsumeMessage_PublishNotificationWithSucess()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        consumer
            .Subscribe(Arg.Any<IEnumerable<string>>());

        consumer
            .Consume(Arg.Any<CancellationToken>())
            .Returns(fixture.GenereteConsumeResult(new Notification_Fake()));

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Setup(f => f.CreateConsumerForMonitor())
            .Returns(consumer);

        fixture.Mocker
            .GetMock<IMediator>()
            .Setup(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.Handlers)
            .Returns(fixture.GenereteHandlers<Notification_Fake>());

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(false);

        // Act

        await monitor.StartAsync(CancellationToken.None);

        // Assert

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When start async, consume event, but is not a notification")]
    public async Task StartAsync_ConsumeMessage_NotPublishNotification()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        consumer
            .Subscribe(Arg.Any<IEnumerable<string>>());

        consumer
            .Consume(Arg.Any<CancellationToken>())
            .Returns(fixture.GenereteConsumeResult(new InvalidNotification_Fake()));

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Setup(f => f.CreateConsumerForMonitor())
            .Returns(consumer);

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.Handlers)
            .Returns(fixture.GenereteHandlers<InvalidNotification_Fake>());

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(false);

        // Act

        await monitor.StartAsync(CancellationToken.None);

        // Assert

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When start async, consume event, but trhow on get handler for topic")]
    public async Task StartAsync_ConsumeMessage_WithSuccess()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        consumer
            .Subscribe(Arg.Any<IEnumerable<string>>());

        consumer
            .Consume(Arg.Any<CancellationToken>())
            .Returns(fixture.GenereteConsumeResult(new InvalidNotification_Fake()));

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Setup(f => f.CreateConsumerForMonitor())
            .Returns(consumer);

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.Handlers)
            .Returns(fixture.GenereteHandlers<Notification_Fake>());

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(false);

        // Act

        var action = monitor.Awaiting(x => x.StartAsync(CancellationToken.None));

        // Assert

        await action.Should().ThrowAsync<InvalidOperationException>();

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When stop async with success")]
    public async Task StopAsync_ConsumeClose_WithSuccess()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        // Act

        var action = monitor.Awaiting(x => x.StopAsync(CancellationToken.None));

        // Assert

        await action.Should().NotThrowAsync();
    }
}
