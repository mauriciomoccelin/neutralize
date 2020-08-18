using AutoMapper;
using Neutralize.Bus;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neutralize.Repositories;
using Neutralize.Tests.Application.Services;

namespace Neutralize.Tests
{
    public abstract class NeutralizeCoreBaseTest : NeutralizeBaseTest
    {
        protected NeutralizeCoreBaseTest()
        {
            services.AddMediatR(typeof(NeutralizeCoreBaseTest).Assembly);
            services.AddAutoMapper(typeof(NeutralizeCoreBaseTest).Assembly);

            services.AddScoped<IInMemoryBus, InMemoryBus>();
            services.AddScoped<IUnitOfWork, NeutralizeCoreDbContext>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Setup In Memory Database
            services.AddDbContext<NeutralizeCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );
            
            // Register DB Context Provider
            services.AddScoped<NeutralizeCoreDbContext>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IPeopleAppService, PeopleAppService>();
            
            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}

