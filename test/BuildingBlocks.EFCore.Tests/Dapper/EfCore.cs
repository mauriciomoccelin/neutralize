using BuildingBlocks.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EFCore.Tests.Dapper
{
    public abstract class EfCore : BuildingBlocksBaseTest
    {
        protected EfCore()
        {
            // Setup In Memory Database
            services.AddDbContext<EfCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );

            // Register DB Context Provider
            services.AddScoped<EfCoreDbContext>();
            
            // Register repositories 
            services.AddScoped<ITodoRepository, TodoEfCoreRepository>();
            
            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}