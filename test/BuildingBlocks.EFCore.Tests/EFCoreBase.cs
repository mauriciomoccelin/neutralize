using BuildingBlocks.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EFCore.Tests
{
    public abstract class EFCoreBase : TestBase
    {
        protected EFCoreBase()
        {
            // Setup In Memory Database
            services.AddDbContext<EfCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );

            // Register DB Context Provider
            services.AddScoped<EfCoreDbContext>();
            
            // Register repositories 
            services.AddScoped<ITodoRepository, TodoRepository>();
            
            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}