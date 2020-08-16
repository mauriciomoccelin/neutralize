using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Neutralize.Http
{
    public class Deserialize : IDeserialize
    {
        public async Task<TData> TryDeserializeAs<TData>(
            HttpResponseMessage response
        )
        {
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TData>(result,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new JsonPrivateOrProtectedPropertyContractResolver()
                    }
                );
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception.Message);
                Console.ResetColor();
                return default(TData);
            }
        }

        public async Task<Result<TData>> GetResult<TData>(
            string uri, 
            HttpResponseMessage response
        )
        {
            var result = await TryDeserializeAs<Result<TData>>(response);
            if (result != null && (result.Success || result?.Errors?.Any() == true))
            {
                return result;
            }

            var data = await TryDeserializeAs<TData>(response);
            return data != null
                ? Result<TData>.Factory.AsSuccess(data) 
                : Result<TData>.Factory.AsError(
                    $"Could not deserialize the return of the resource {uri}."
                );
        }
        
        public sealed class JsonPrivateOrProtectedPropertyContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(
                MemberInfo member, 
                MemberSerialization memberSerialization
            )
            {
                var prop = base.CreateProperty(member, memberSerialization);

                if (!prop.Writable)
                {
                    var property = member as PropertyInfo;
                    if (property != null)
                    {
                        var hasPrivateOrProtectedSetter = property.GetSetMethod(true) != null;
                        prop.Writable = hasPrivateOrProtectedSetter;
                    }
                }

                return prop;
            }
        }
    }
}
