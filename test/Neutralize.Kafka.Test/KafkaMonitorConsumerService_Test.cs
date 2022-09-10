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
    [Fact(DisplayName = "When consume event but EnableMonitorHandler is disabled")]
    public async Task Consume_ConsumeWithEnableMonitorHandlerDisabled_MustIgnoreConsume()
    {
        // Arrange
        var monitorService = fixture.GenereteKafkaMonitorConsumerService();

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(false);

        // Act
        await monitorService.Consume(CancellationToken.None);

        // Arrange
        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Never);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When consume but cancellation is requested must throw operation canceled")]
    public async Task Consume_CancellationRequested_MustThrowOperationCanceledException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var consumer = fixture.GenereteKafkaConsumer();
        var monitorService = fixture.GenereteKafkaMonitorConsumerService();

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(true);

        // Act
        await monitorService.Consume(cts.Token);

        // Arrange
        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Never);
    }


    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When consume event as notification")]
    public async Task Consume_ValidNotification_PublishNotificationWithSucess()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitorService = fixture.GenereteKafkaMonitorConsumerService();

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(true);

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

        // Act

        await monitorService.Consume(CancellationToken.None);

        // Assert

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When consume event, but is not a notification")]
    public async Task Consume_InvalidNotification_NeverPublishNotification()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(true);

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

        // Act

        await monitor.Consume(CancellationToken.None);

        // Assert

        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When consume event, but has a invalid handler for topic")]
    public async Task Consume_InvalidHandlerTopic_MustThrowInvalidOperationException()
    {
        var consumer = fixture.GenereteKafkaConsumer();
        var monitor = fixture.GenereteKafkaMonitorConsumerService();

        fixture.Mocker
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.EnableMonitorHandler)
            .Returns(true);

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
            .GetMock<IKafkaConfiguration>()
            .SetupGet(f => f.Handlers)
            .Returns(fixture.GenereteHandlers<InvalidNotification_Fake>());

        // Act
        await monitor.Consume(CancellationToken.None);

        // Assert
        fixture.Mocker
            .GetMock<IKafkaFactory>()
            .Verify(f => f.CreateConsumerForMonitor(), Times.Once);

        fixture.Mocker
            .GetMock<IMediator>()
            .Verify(f => f.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
