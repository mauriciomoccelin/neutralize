using Xunit;

namespace Neutralize.Kafka.Test;

[CollectionDefinition(nameof(KafkaFixtureCollection))]
public class KafkaFixtureCollection : ICollectionFixture<KafkaFixture> { }
