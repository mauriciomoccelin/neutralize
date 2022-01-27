using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neutralize.Json
{
    public sealed class JsonPrivateOrProtectedPropertyContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (prop.Writable) return prop;
            var property = member as PropertyInfo;
            if (property == null) return prop;
            var hasPrivateOrProtectedSetter = property.GetSetMethod(true) != null;
            prop.Writable = hasPrivateOrProtectedSetter;

            return prop;
        }
    }
}
