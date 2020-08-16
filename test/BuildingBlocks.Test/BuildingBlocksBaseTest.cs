using System;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Tests
{
    /// <summary>
    /// Abstract class for testing
    /// </summary>
    public abstract class BuildingBlocksBaseTest : IDisposable
    {
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;

        protected BuildingBlocksBaseTest()
        {
            services = new ServiceCollection();
        }
        
        /// <summary>
        /// For disposed object
        /// </summary>
        public abstract void Dispose();
        
        /// <summary>
        /// Resolve implementation for the service T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Resolve<T>() { return provider.GetRequiredService<T>(); }
    }
}