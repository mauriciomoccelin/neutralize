using AutoMapper;
using Neutralize.Bus;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Neutralize.Tests
{
    public abstract class NeutralizeCoreNeutralizeBaseTest : NeutralizeBaseTest
    {
        protected NeutralizeCoreNeutralizeBaseTest()
        {
            services.AddMediatR(typeof(NeutralizeCoreNeutralizeBaseTest).Assembly);
            services.AddAutoMapper(typeof(NeutralizeCoreNeutralizeBaseTest).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInMemoryBus, InMemoryInMemoryBus>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Setup In Memory Database
            services.AddDbContext<NeutralizeCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );

            // Register DB Context Provider
            services.AddScoped<NeutralizeCoreDbContext>();

            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}

