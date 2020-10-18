using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neutralize.Kafka.Helpers
{
    internal static class ObjectExtensions
    {
        public static Message<Null, string> ToMessage(this object @object)
        {
            return new Message<Null, string>()
            {
                Value = JsonConvert.SerializeObject(@object, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None
                })
            };
        }
    }
}
