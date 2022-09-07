using Xunit;
using Confluent.Kafka;
using FluentAssertions;
using Newtonsoft.Json;

namespace Neutralize.Kafka.Test;

[Collection(nameof(KafkaFixtureCollection))]
public class KafkaJsonDeserializer_Test
{
    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When deserialize a valid payload with success")]
    public void Deserialize_ValidPayload_WithSuccess()
    {
        // Arrange
        var payload = new Notification_Fake();
        var context = new SerializationContext();
        var serializer = new KafkaJsonSerializer<Notification_Fake>();
        var deserializer = new KafkaJsonDeserializer<Notification_Fake>();

        var data = serializer.Serialize(payload, context);

        // Act
        var result = deserializer.Deserialize(data, false, context);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(payload);
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When deserialize a valid payload as string, must Throw JsonReaderException")]
    public void Deserialize_ValidPayloadAsString_ThrowJsonReaderException()
    {
        // Arrange
        var payload = new Notification_Fake();
        var context = new SerializationContext();
        var serializer = new KafkaJsonSerializer<Notification_Fake>();
        var deserializer = new KafkaJsonDeserializer<string>();

        var data = serializer.Serialize(payload, context);

        // Act
        var action = deserializer.Invoking(x => x.Deserialize(data, false, context));

        // Assert
        action.Should().ThrowExactly<JsonReaderException>();
    }

    [Trait("Data", "Kafka")]
    [Fact(DisplayName = "When deserialize a null payload must return a null of type")]
    public void Deserialize_NullPayload_MustReturnDefaultOfType()
    {
        // Arrange
        var payload = new Notification_Fake();
        var context = new SerializationContext();
        var serializer = new KafkaJsonSerializer<Notification_Fake>();
        var deserializer = new KafkaJsonDeserializer<Notification_Fake>();

        var data = serializer.Serialize(payload, context);

        // Act
        var result = deserializer.Deserialize(data, true, context);

        // Assert
        result.Should().BeNull();
        result.Should().BeEquivalentTo(default(Notification_Fake));
    }
}