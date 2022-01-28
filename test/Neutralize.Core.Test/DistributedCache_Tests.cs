using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace Neutralize.Core.Test
{
    [Collection(nameof(NeutralizeCoreCollection))]
    public class DistributedCache_Tests
    {
        private readonly IDistributedCache cache;
        private readonly NeutralizeCoreFixture fixture;
        
        public DistributedCache_Tests(NeutralizeCoreFixture fixture)
        {
            this.fixture = fixture;
            var services = new ServiceCollection()
                .AddDistributedMemoryCache();

            var sp = services.BuildServiceProvider();
            cache = sp.GetRequiredService<IDistributedCache>();
        }
        
        [Trait("Category", "Core - DistributedCache")]
        [Fact(DisplayName = "Get cache value using distributed cache extensions")]
        public async void DistributedCacheExtension_GetValue_WithSuccess()
        {
            // Arrange
            var value = fixture.Faker.Lorem.Words();
            await cache.SetStringAsync(nameof(value), JsonConvert.SerializeObject(value));

            // Act
            var cacheValue = await cache.GetValue(Array.Empty<string>, nameof(value));

            // Assert
            Assert.Equal(value.Length, cacheValue.Length);
        }
        
        [Trait("Category", "Core - DistributedCache")]
        [Fact(DisplayName = "Set cache value using distributed cache extensions")]
        public async void DistributedCacheExtension_SetValue_WithSuccess()
        {
            // Arrange
            var value = fixture.Faker.Lorem.Words();
            
            // Act
            await cache.SetValue(value, nameof(value));

            // Assert
            var cacheValue = await cache.GetValue(Array.Empty<string>, nameof(value));
            Assert.Equal(value.Length, cacheValue.Length);
        }
        
        [Trait("Category", "Core - DistributedCache")]
        [Fact(DisplayName = "Remove cache value using distributed cache extensions")]
        public async void DistributedCacheExtension_RemoveValue_WithSuccess()
        {
            // Arrange
            var value = fixture.Faker.Lorem.Words();
            await cache.SetValue(value, nameof(value));
            
            // Act
            await cache.RemoveValue(nameof(value));
            
            // Assert
            var cacheValue = await cache.GetValue(Array.Empty<string>, nameof(value));
            Assert.Empty(cacheValue);
        }
    }
}