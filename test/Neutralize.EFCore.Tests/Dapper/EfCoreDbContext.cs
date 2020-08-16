using System.Threading.Tasks;
using Neutralize.UoW;
using Microsoft.EntityFrameworkCore;

namespace Neutralize.EFCore.Tests.Dapper
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
