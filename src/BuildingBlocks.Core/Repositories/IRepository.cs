using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Repositories
{
    public interface IRepository<TEntity, in TId> : IDisposable 
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(TId id);
    }
}
