using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neutralize.Models;

namespace Neutralize.Repositories
{
    public interface IRepository<TEntity, in TId> : IDisposable
        where TEntity : IEntity
        where TId : struct
    {
        Task<bool> Commit();
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(TId id);
        IQueryable<TEntity> GetAll();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrAsync(Expression<Func<TEntity, bool>> predicate, TEntity @default = default(TEntity));
    }
}
