using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Data.EFCore
{
    public abstract class EFCoreRepository<TDbContext, TEntity, TId> : 
        IEFCoreRepository<TEntity, TId> where TDbContext: DbContext 
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        protected TDbContext EfCoreDb { get; }
        protected DbSet<TEntity> DbSet { get; }

        protected EFCoreRepository(TDbContext efCoreDb)
        {
            EfCoreDb = efCoreDb;
            DbSet = EfCoreDb.Set<TEntity>();
        }

        public void Dispose()
        {
            EfCoreDb.Dispose();
        }

        public async Task<bool> Commit()
        {
            return await EfCoreDb.SaveChangesAsync() > 0;
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual Task<IQueryable<TEntity>> GetAll()
        {
            return Task.FromResult(DbSet.AsQueryable());
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

        public Task<int> SaveChangesAsync() => EfCoreDb.SaveChangesAsync();
    }
}