using Xunit;
using FluentAssertions;
using System;

namespace Neutralize.Kafka.Test;

[Collection(nameof(KafkaFixtureCollection))]
public class KafkaConfiguration_Test
{
    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set flush timeout with value greater than zero")]
    public void SetFlushTimeout_GreaterThanZero_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();
        var assertValue = TimeSpan.FromSeconds(byte.MaxValue);

        // Act
        configuration.SetFlushTimeout(byte.MaxValue);

        // Assert
        configuration.FlushTimeout.Should().Be(assertValue);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set flush timeout with value less than zero")]
    public void SetFlushTimeout_LessThanZero_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.SetFlushTimeout(byte.MinValue));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Theory(DisplayName = "When set enable monitor with bollean value")]
    [InlineData(true)]
    [InlineData(false)]
    public void EnableMonitor_TrueOrFalse_MustBeSuccess(bool enable)
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        configuration.EnableMonitor(enable);

        // Assert
        configuration.EnableMonitorHandler.Should().Be(enable);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set enable monitor with default value true")]
    public void EnableMonitor_DefaultValue_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        configuration.EnableMonitor();

        // Assert
        configuration.EnableMonitorHandler.Should().Be(true);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set consumer config with group empty must throw exception")]
    public void SetConsumerConfig_EmptyGroup_ThrowArgumentException()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.SetConsumerConfig(string.Empty, "localhost:9092"));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set consumer config with bootstrap servers empty must throw exception")]
    public void SetConsumerConfig_EmptyBootstrapServers_ThrowArgumentException()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.SetConsumerConfig("group", string.Empty));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set consumer config with group and bootstrap valid must be success")]
    public void SetConsumerConfig_GroupAndBootstrapServers_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        configuration.SetConsumerConfig("group", "localhost:9092");

        // Assert
        configuration.ConsumerConfig.GroupId.Should().Be("group");
        configuration.ConsumerConfig.BootstrapServers.Should().Be("localhost:9092");
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set producer config with bootstrap servers empty must throw exception")]
    public void SetProducerConfig_EmptyBootstrapServers_ThrowArgumentException()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.SetProducerConfig(string.Empty));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When set producer bootstrap valid must be success")]
    public void SetProducerConfig_BootstrapServers_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        configuration.SetProducerConfig("localhost:9092");

        // Assert
        configuration.ProducerConfig.BootstrapServers.Should().Be("localhost:9092");
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When add handler with invalid type must throw exception")]
    public void AddHandler_BootstrapServers_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.AddHandler("topic", typeof(InvalidNotification_Fake)));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When add handler with valid type must throw exception")]
    public void AddHandler_InvalidTopic_ThrowArgumentException()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.AddHandler(string.Empty, typeof(Notification_Fake)));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When add handler with valid type must throw exception")]
    public void AddHandler_InvalidNotificationType_ThrowArgumentException()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        var action = configuration.Invoking(x => x.AddHandler("topic", typeof(InvalidNotification_Fake)));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When add handler with valid type must throw exception")]
    public void AddHandler_ValidTopicAndNotificationType_MustBeSuccess()
    {
        // Arrange
        var configuration = KafkaConfiguration.Create();

        // Act
        configuration.AddHandler("topic", typeof(Notification_Fake));

        // Assert
        configuration.Handlers.Keys.Should().Contain("topic");
        configuration.Handlers.Values.Should().Contain(typeof(Notification_Fake));
    }
}