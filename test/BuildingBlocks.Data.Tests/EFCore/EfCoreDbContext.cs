using System.Threading.Tasks;
using BuildingBlocks.Core.UoW;
using BuildingBlocks.Data.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Data.Tests.EFCore
{
    public class EfCoreDbContext : DbContext, IUnitOfWork
    {
        private DbSet<ToDo> ToDos { get; set; }

        public EfCoreDbContext(
            DbContextOptions<EfCoreDbContext> options
        ) : base(options)
        {
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}