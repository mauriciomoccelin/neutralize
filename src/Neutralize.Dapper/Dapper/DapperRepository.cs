using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Neutralize.Repositories;
using Dapper;
using Neutralize.Application;
using Optional;

namespace Neutralize.Dapper
{
    public abstract class DaperRepository : IDapperRepository
    {
        protected readonly DynamicParameters parameters;

        protected DaperRepository()
        {
            parameters = new DynamicParameters();
        }

        protected void AddParameter(string name, object value) => parameters.Add(name, value);
        
        public void Dispose() => GC.SuppressFinalize(this);
        protected abstract Task<Option<T>> ConnectionWrapper<T>(Func<IDbConnection, Task<Option<T>>> action);
        
        /// <summary>
        /// Using QueryMultipleAsync split query by (;) Ex: command count; command items.
        /// Always command count first, before command items.
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>PagedResultDto<T /></returns>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        public virtual async Task<Option<PagedResultDto<T>>> QueryPaged<T>(string command) where T : class
        {
            return await ConnectionWrapper(async connection =>
            {
                var query = await connection.QueryMultipleAsync(command, parameters);

                var count = query.Read<long>().FirstOrDefault();
                var items = query.Read<T>().ToList();
                
                var page = new PagedResultDto<T>(count, items);
                return Option.Some(page);
            });
        }
    }
}
