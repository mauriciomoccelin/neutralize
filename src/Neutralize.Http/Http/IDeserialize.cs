using System.Net.Http;
using System.Threading.Tasks;

namespace Neutralize.Http
{
    public interface IDeserialize
    {
        Task<TData> TryDeserializeAs<TData>(HttpResponseMessage response);
        Task<Result<TData>> GetResult<TData>(string uri, HttpResponseMessage response);
    }
}
