using BuildingBlocks.Domain.Tests.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BuildingBlocks.Domain.Tests
{
    public abstract class TestBase
    {
        private readonly IServiceProvider provider;
        private readonly IServiceCollection services;

        public TestBase()
        {
            services = new ServiceCollection();
            services.AddMediatR(typeof(TestBase).Assembly);

            services.AddScoped<IBus, InMemoryBus>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            provider = services.BuildServiceProvider();
        }

        public T Resolve<T>() { return provider.GetRequiredService<T>(); }
    }
}
