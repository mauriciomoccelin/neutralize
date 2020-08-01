using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Repositories
{
    public interface IEFCoreRepository<TEntity, in TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        Task<bool> Commit();
        IQueryable<TEntity> GetAll();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrAsync(Expression<Func<TEntity, bool>> predicate, TEntity @default = null);
    }
}