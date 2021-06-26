using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Neutralize.Dapper.Extensions.Filters;
using Neutralize.Dapper.Extensions.Sort;
using Neutralize.Models;
using Neutralize.Repositories;
using Dapper;
using DapperExtensions;
using Neutralize.Application;

namespace Neutralize.Dapper
{
    public abstract class DapperRepository<TEntity, TId> : IDapperRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : struct
    {
        public DbConnection Connection { get; }
        public Task<TEntity> GetAsync(TId id)
        {
            var entity = Connection.Get<TEntity>(id);
            return Task.FromResult(entity);
        }

        public async Task<TEntity> FirstOrAsync(
            Expression<Func<TEntity, bool>> predicate,
            TEntity @default = default
        )
        {
            var getAllResult = await GetAllAsync(predicate, true, x => x.Id);
            var enumerable = getAllResult as TEntity[] ?? getAllResult.ToArray();
            return enumerable.Any() ? enumerable.First() : @default;
        }

        public DynamicParameters Parameters { get; }

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

        public Task<long> CountAsync(
            Expression<Func<TEntity, bool>> predicate
        )
        {
            var _predicate = predicate.ToPredicateGroup<TEntity, TId>();
            var result = Connection.Count<TEntity>(_predicate);
            return Task.FromResult<long>(result);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var result = Connection.GetList<TEntity>();
            return Task.FromResult(result);
        }

        public Task<PagedResultDto<TEntity>> GetAllPagedAsync(
            int page,
            int itemsPerPage,
            Expression<Func<TEntity, bool>> predicate,
            bool ascending = true,
            params Expression<Func<TEntity, object>>[] sort
        )
        {
            var _sort = sort.ToSortable<TEntity>(ascending).ToList();
            var _predicate = predicate.ToPredicateGroup<TEntity, TId>();

            var total = Connection.Count<TEntity>(_predicate);
            var result = Connection.GetPage<TEntity>(_predicate, _sort, page, itemsPerPage);

            var pageResult = new PagedResultDto<TEntity>(total, result);

            return Task.FromResult(pageResult);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool ascending = true,
            params Expression<Func<TEntity, object>>[] sort
        )
        {
            var _sort = sort.ToSortable<TEntity>(ascending).ToList();
            var _predicate = predicate.ToPredicateGroup<TEntity, TId>();

            var result = Connection.GetList<TEntity>(_predicate, _sort);

            return Task.FromResult(result);
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
