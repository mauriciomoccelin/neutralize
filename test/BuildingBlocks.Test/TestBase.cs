using System;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Test
{
    /// <summary>
    /// Abstract class for testing
    /// </summary>
    public abstract class TestBase : IDisposable
    {
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;

        protected TestBase()
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