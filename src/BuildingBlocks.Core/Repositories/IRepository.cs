using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Repositories
{
    public interface IRepository<TEntity, in TId> : IDisposable where TEntity : class where TId : struct
    {
        Task<TEntity> GetById(TId id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(Guid id);
        Task<int> SaveChanges();
    }
}
