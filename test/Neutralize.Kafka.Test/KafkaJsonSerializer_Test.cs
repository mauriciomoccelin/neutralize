using Xunit;
using Confluent.Kafka;
using FluentAssertions;

namespace Neutralize.Kafka.Test;

[Collection(nameof(KafkaFixtureCollection))]
public class KafkaJsonSerializer_Test
{
    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When serialize a valid payload with success")]
    public void Serialize_ValidPayload_WithSuccess()
    {
        // Arrange
        var serializer = new KafkaJsonSerializer<Notification_Fake>();

        // Act
        var result = serializer.Serialize(new Notification_Fake(), new SerializationContext());

        // Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<byte[]>();
    }
}