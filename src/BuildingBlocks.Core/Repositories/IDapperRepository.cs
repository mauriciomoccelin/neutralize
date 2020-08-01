using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Repositories
{
    public interface IDapperRepository<TEntity, in TId> : IRepository<TEntity, TId> 
        where TEntity : Entity<TEntity, TId> 
        where TId : struct
    {
        DbConnection Connection { get; }
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
