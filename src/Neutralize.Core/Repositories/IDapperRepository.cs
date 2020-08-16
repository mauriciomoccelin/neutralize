using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neutralize.Models;

namespace Neutralize.Repositories
{
    public interface IDapperRepository<TEntity, in TId> : IDisposable
        where TEntity : Entity
        where TId : struct
    {
        DbConnection Connection { get; }
        Task AddAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrAsync(Expression<Func<TEntity, bool>> predicate, TEntity @default = null);
        Task<IEnumerable<TEntity>> GetAllPagedAsync(
            int page,
            int itemsPerPage,
            Expression<Func<TEntity, bool>> predicate,
            bool ascending = true,
            params Expression<Func<TEntity, object>>[] sort
        );
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool ascending = true,
            params Expression<Func<TEntity, object>>[] sort
        );
    }
}

