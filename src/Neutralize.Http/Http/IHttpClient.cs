using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neutralize.Http
{
    public interface IHttpClient : IDisposable
    {
        #region Get
        Task<Result<TData>> GetAsync<TData>(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        #endregion

        #region Post
        Task<Result> PostAsync(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        
        Task<Result<TData>> PostAsync<TPostData, TData>(
            string uri,
            TPostData data,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        
        Task<Result> PostAsync<TPostData>(
            string uri,
            TPostData data,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );

        Task<Result<TData>> PostAsync<TData>(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        #endregion
        
        #region Put
        Task<Result> PutAsync(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        
        Task<Result<TData>> PutAsync<TPutData, TData>(
            string uri,
            TPutData data,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        
        Task<Result> PutAsync<TPutData>(
            string uri,
            TPutData data,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );

        Task<Result<TData>> PutAsync<TData>(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        #endregion

        #region Delete
        Task<Result> DeleteAsync(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        );
        #endregion
    }
}
