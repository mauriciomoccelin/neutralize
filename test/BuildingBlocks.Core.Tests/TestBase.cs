using BuildingBlocks.Core.Tests.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;

namespace BuildingBlocks.Core.Tests
{
    public abstract class TestBase
    {
        private readonly IServiceProvider provider;

        protected TestBase()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddMediatR(typeof(TestBase).Assembly);

            services.AddScoped<IInMemoryBus, InMemoryInMemoryBus>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            provider = services.BuildServiceProvider();
        }

        protected T Resolve<T>() { return provider.GetRequiredService<T>(); }
    }
}
