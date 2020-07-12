using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    public interface IRepository<TEntity, TId> : IDisposable where TEntity : class where TId : struct
    {
        Task<TEntity> GetById(TId id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Remove(Guid id);
        Task<int> SaveChanges();
    }
}
