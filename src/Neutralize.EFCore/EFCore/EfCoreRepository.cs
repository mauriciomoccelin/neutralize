using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neutralize.Models;
using Neutralize.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Neutralize.EFCore
{
    public abstract class EfCoreRepository<TDbContext, TEntity, TId> :
        IRepository<TEntity, TId> where TDbContext : DbContext
        where TEntity : Entity<TId>
        where TId : struct
    {
        protected TDbContext Context { get; }
        protected DbSet<TEntity> DbSet { get; }

        protected EfCoreRepository(TDbContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public async Task<bool> Commit()
        {
            var success = await Context.SaveChangesAsync() > 0;
            return success;
        }

        public async Task<TEntity> GetAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public Task<long> CountAsync(
            Expression<Func<TEntity, bool>> predicate
        )
        {
            return DbSet.LongCountAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate
        )
        {
            var query = DbSet.Where(predicate);
            return await query.ToListAsync();
        }

        public Task<TEntity> FirstOrAsync(
            Expression<Func<TEntity, bool>> predicate,
            TEntity @default = default(TEntity)
        )
        {
            var entity = DbSet.Where(predicate).FirstOrDefault();
            return Task.FromResult(entity ?? @default);
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            return Task.FromResult(DbSet.Remove(entity));
        }

        public virtual Task AddAsync(TEntity entity)
        {
            return Task.FromResult(DbSet.Add(entity));
        }

        public virtual Task UpdateAsync(TEntity entity)
        {
            return Task.FromResult(DbSet.Update(entity));
        }
    }
}
