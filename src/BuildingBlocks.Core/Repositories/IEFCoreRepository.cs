using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Repositories
{
    public interface IEFCoreRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        Task<bool> Commit();
        Task<IQueryable<TEntity>> GetAll();
    }
}