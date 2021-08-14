using System;
using System.Data;
using System.Threading.Tasks;
using Neutralize.Repositories;
using Dapper;
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
    }
}
