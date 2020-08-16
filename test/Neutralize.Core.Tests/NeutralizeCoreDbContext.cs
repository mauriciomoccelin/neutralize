using System.Threading.Tasks;
using Neutralize.UoW;
using Microsoft.EntityFrameworkCore;

namespace Neutralize.Tests
{
    public class NeutralizeCoreDbContext : DbContext, IUnitOfWork
    {
        public NeutralizeCoreDbContext(
            DbContextOptions<NeutralizeCoreDbContext> options
        ) : base(options)
        {
        }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
