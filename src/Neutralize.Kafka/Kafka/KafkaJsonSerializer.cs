using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neutralize.Kafka
{
    public sealed class KafkaJsonSerializer<TValue> : ISerializer<TValue>
    {
        public byte[] Serialize(TValue data, SerializationContext context)
        {
            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
            });

            return Encoding.UTF8.GetBytes(json);
        }
    }
}