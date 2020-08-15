using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;
using BuildingBlocks.Test;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Tests
{
    public abstract class BuildingBlocksCoreBaseTest : TestBase
    {
        protected BuildingBlocksCoreBaseTest()
        {
            services.AddMediatR(typeof(BuildingBlocksCoreBaseTest).Assembly);
            services.AddAutoMapper(typeof(BuildingBlocksCoreBaseTest).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInMemoryBus, InMemoryInMemoryBus>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Setup In Memory Database
            services.AddDbContext<BuildingBlocksCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );

            // Register DB Context Provider
            services.AddScoped<BuildingBlocksCoreDbContext>();

            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}
