using System;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Neutralize.Kafka
{
    public sealed class KafkaJsonDeserializer<TValue> : IDeserializer<TValue>
    {
        public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (!isNull)
            {
                var json = System.Text.Encoding.UTF8.GetString(data);

                return JsonConvert.DeserializeObject<TValue>(json);
            }

            return default;
        }
    }
}