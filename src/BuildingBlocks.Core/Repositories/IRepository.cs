using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Repositories
{
    public interface IRepository<TEntity, in TId>
        : IDisposable where TId : struct where TEntity : class
    {
        Task<TEntity> GetByIdAsync(TId id);
        Task<IQueryable<TEntity>> GetAll();
        
        Task RemoveAsync(TEntity entity);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entity);
        Task AddRangeAsync(IEnumerable<TEntity> entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entity);
    }
}
