using Newtonsoft.Json;

namespace Neutralize.Json
{
    public sealed class CustomJsonSerializerSettings : JsonSerializerSettings
    {
        public CustomJsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore;
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            ContractResolver = new JsonPrivateOrProtectedPropertyContractResolver();
        }

        public static CustomJsonSerializerSettings Create() => new();
    }
}
