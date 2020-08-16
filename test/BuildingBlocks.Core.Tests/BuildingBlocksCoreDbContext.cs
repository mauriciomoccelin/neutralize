using System.Threading.Tasks;
using BuildingBlocks.UoW;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Tests
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