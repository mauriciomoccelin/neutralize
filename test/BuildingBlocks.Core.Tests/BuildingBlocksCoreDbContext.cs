using System.Threading.Tasks;
using BuildingBlocks.Core.UoW;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Tests
{
    public class BuildingBlocksCoreDbContext : DbContext, IUnitOfWork
    {
        public BuildingBlocksCoreDbContext(
            DbContextOptions<BuildingBlocksCoreDbContext> options
        ) : base(options)
        {
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}