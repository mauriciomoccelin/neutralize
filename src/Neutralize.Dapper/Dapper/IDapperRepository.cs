using System;
using System.Threading.Tasks;
using Neutralize.Application;
using Optional;

namespace Neutralize.Dapper
{
    public interface IDapperRepository : IDisposable
    {
        void AddParameter(string name, object value);
        Task<Option<T>> First<T>(string command);
        Task<Option<PagedResultDto<T>>> Paged<T>(string command);
    }
}

