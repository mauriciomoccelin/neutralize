using Neutralize.EFCore;
using Neutralize.Models;

namespace Neutralize.Tests
{
    public class Repository<TEntity, TId> : EfCoreRepository<NeutralizeCoreDbContext, TEntity, TId>
        where TEntity : Entity
        where TId : struct
    {
        public Repository(NeutralizeCoreDbContext context) : base(context)
        {
        }
    }
}