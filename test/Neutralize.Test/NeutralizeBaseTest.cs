using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Neutralize.Tests
{
    /// <summary>
    /// Abstract class for testing
    /// </summary>
    public abstract class NeutralizeBaseTest : IDisposable
    {
        protected IServiceProvider provider;
        protected readonly IServiceCollection services;
        protected readonly IConfiguration configuration;

        protected NeutralizeBaseTest()
        {
            services = new ServiceCollection();
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
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
