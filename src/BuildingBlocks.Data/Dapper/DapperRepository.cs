using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Core.Repositories;
using Dapper;
using DapperExtensions;

namespace BuildingBlocks.Data.Dapper
{
    public abstract class DapperRepository<TEntity, TId> : IDapperRepository<TEntity, TId> 
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        public DbConnection Connection { get; }
        public  DynamicParameters Parameters { get; }

        protected DapperRepository(DbConnection connection)
        {
            Connection = connection;
            Parameters = new DynamicParameters();
        }
        
        public void Dispose()
        {
            Connection.Dispose();
        }

        public void AddParameter(string name, object value)
        {
            Parameters.Add(name, value);
        }

        public Task<TEntity> GetByIdAsync(TId id)
        {
            var entity = Connection.Get<TEntity>(id);
            return Task.FromResult(entity);
        }
        
        public Task<IEnumerable<TEntity>> GetAll()
        {
            var entity = Connection.GetList<TEntity>();
            return Task.FromResult(entity); 
        }

        public Task RemoveAsync(TEntity entity)
        {
            Connection.Delete<TEntity>(entity);
            return Task.CompletedTask;
        }

        public Task AddAsync(TEntity entity)
        {
            Connection.Insert(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity entity)
        {
            Connection.Update(entity);
            return Task.CompletedTask;         
        }
    }
}