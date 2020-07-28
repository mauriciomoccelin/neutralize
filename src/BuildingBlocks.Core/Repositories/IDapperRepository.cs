using System;
using System.Data;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Repositories
{
    public interface IDapperRepository<out TParameter>
    {
        TParameter Parameters { get; }
        IDbConnection Connection { get; }
        void AddParameter(string name, object value);
        Task<T> ConnectionWrapper<T>(Func<IDbConnection, Task<T>> func);
    }
}
