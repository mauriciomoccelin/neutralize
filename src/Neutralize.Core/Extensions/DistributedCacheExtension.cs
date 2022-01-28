using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Neutralize.Json;
using Newtonsoft.Json;
using Optional;

namespace Neutralize.Extensions
{
    public static class DistributedCacheExtension
    {
        public static string GetCacheKey(params string[] keys) => string.Join(":", keys);

        public static async Task<TValue> GetValue<TValue>(
            this IDistributedCache distributedCache,
            Func<TValue> factoryOption,
            params string[] keys
        )
        {
            var cacheKey = GetCacheKey(keys);
            var cache = await distributedCache.GetStringAsync(cacheKey);
            var option = TryDeserialize<TValue>(cache);

            return option.ValueOr(factoryOption);
        }

        public static async Task SetValue<TValue>(
            this IDistributedCache distributedCache,
            TValue value,
            params string[] keys
        )
        {
            var cacheKey = GetCacheKey(keys);
            await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(value));
        }

        public static async Task RemoveValue(
            this IDistributedCache distributedCache,
            params string[] keys
        )
        {
            var cacheKey = GetCacheKey(keys);
            await distributedCache.RemoveAsync(cacheKey);
        }

        public static Option<TValue> TryDeserialize<TValue>(this string cache)
        {
            try
            {
                var value = JsonConvert.DeserializeObject<TValue>(
                    cache,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new JsonPrivateOrProtectedPropertyContractResolver()
                    }
                );

                return Option.Some(value);
            }
            catch
            {
                return Option.None<TValue>();
            }
        }
    }
}
