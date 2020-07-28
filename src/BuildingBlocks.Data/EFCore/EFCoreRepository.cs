using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Data.EFCore
{
    public abstract class EFCoreRepository<TDbContext, TEntity, TId> : 
        IEFCoreRepository<TEntity, TId> where TDbContext: DbContext 
        where TEntity : class 
        where TId : struct
    {
        protected TDbContext Db { get; }
        protected DbSet<TEntity> DbSet { get; }

        protected EFCoreRepository(TDbContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
        
        public virtual async Task<TEntity> GetByIdAsync(TId id) => await DbSet.FindAsync(id);
        
        public virtual Task<IQueryable<TEntity>> GetAll() => Task.FromResult(DbSet.AsQueryable());
        
        public virtual Task RemoveAsync(TEntity entity) => Task.FromResult(DbSet.Remove(entity));
        
        public virtual Task AddAsync(TEntity entity) => Task.FromResult(DbSet.Add(entity));

        public virtual Task UpdateAsync(TEntity entity) => Task.FromResult(DbSet.Update(entity));

        public virtual Task UpdateRangeAsync(IEnumerable<TEntity> entity)
        {
            DbSet.RemoveRange(entity);
            return Task.CompletedTask;
        }
        
        public virtual Task AddRangeAsync(IEnumerable<TEntity> entity)
        {
            return DbSet.AddRangeAsync(entity);
        }
        
        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entity)
        {
            DbSet.UpdateRange(entity);
            return Task.CompletedTask;
        }
        
        public Task<int> SaveChangesAsync() => Db.SaveChangesAsync();
    }
}