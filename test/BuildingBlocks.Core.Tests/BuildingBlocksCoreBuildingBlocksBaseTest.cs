using AutoMapper;
using BuildingBlocks.Bus;
using BuildingBlocks.Notifications;
using BuildingBlocks.UoW;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Tests
{
    public abstract class BuildingBlocksCoreBuildingBlocksBaseTest : BuildingBlocksBaseTest
    {
        protected BuildingBlocksCoreBuildingBlocksBaseTest()
        {
            services.AddMediatR(typeof(BuildingBlocksCoreBuildingBlocksBaseTest).Assembly);
            services.AddAutoMapper(typeof(BuildingBlocksCoreBuildingBlocksBaseTest).Assembly);

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
