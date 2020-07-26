using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BuildingBlocks.Http
{
    public sealed class DefaultHttpClient : IHttpClient
    {
        #region Attributes
        
        private readonly IDeserialize deserialize;
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly HttpClient httpClient;
        private readonly HttpClientHandler httpClientHandler;
        #endregion

        #region Life Cycle

        public DefaultHttpClient(
            IDeserialize deserialize,
            IHttpContextAccessor httpContextAccessor
        )
        { 
            this.deserialize = deserialize;
            this.httpContextAccessor = httpContextAccessor;
            
            httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (
                    message, cert, chain, errors
                ) => true
            };

            httpClient = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromMinutes(15)
            };
        }

        public void Dispose()
        {
            httpClient?.Dispose();
            httpClientHandler?.Dispose();
        }
        
        #endregion

        #region Get

        public Task<Result<TData>> GetAsync<TData>(
            string uri, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using (
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization)
            )
            {
                return GetResultFromRequest<TData>(uri, requestMessage, cancellationToken);   
            }
        }

        #endregion

        #region Post

        public Task<Result> PostAsync(
            string uri,
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            return GetResultFromRequest(uri, requestMessage, cancellationToken);
        }

        public Task<Result<TData>> PostAsync<TPostData, TData>(
            string uri, 
            TPostData data, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
                
            requestMessage.Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8, "application/json"
            );
            
            return GetResultFromRequest<TData>(uri, requestMessage, cancellationToken);
        }

        public Task<Result> PostAsync<TPostData>(
            string uri, 
            TPostData data, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
                
            requestMessage.Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8, "application/json"
            );
            
            return GetResultFromRequest(uri, requestMessage, cancellationToken);
        }

        public Task<Result<TData>> PostAsync<TData>(
            string uri, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            return GetResultFromRequest<TData>(uri, requestMessage, cancellationToken);
        }        

        #endregion

        #region Put

        public Task<Result> PutAsync(
            string uri, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            return GetResultFromRequest(uri, requestMessage, cancellationToken);
        }

        public Task<Result<TData>> PutAsync<TPutData, TData>(
            string uri, 
            TPutData data, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            requestMessage.Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8, "application/json"
            );
            
            return GetResultFromRequest<TData>(uri, requestMessage, cancellationToken);
        }

        public Task<Result> PutAsync<TPutData>(
            string uri, 
            TPutData data, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            requestMessage.Content = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8, "application/json"
            );
            
            return GetResultFromRequest(uri, requestMessage, cancellationToken);
        }

        public Task<Result<TData>> PutAsync<TData>(
            string uri, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            return GetResultFromRequest<TData>(uri, requestMessage, cancellationToken);
        }        

        #endregion

        #region Delete

        public Task<Result> DeleteAsync(
            string uri, 
            Authorization authorization = default(Authorization),
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri)
                .CopyAuthorizationHeaderFrom(httpContextAccessor.HttpContext)
                .Apply(authorization);
            
            return GetResultFromRequest(uri, requestMessage, cancellationToken);
        }

        #endregion
        
        #region Private
        private async Task<Result> GetResultFromRequest(
            string uri,
            HttpRequestMessage requestMessage,
            CancellationToken cancellationToken
        )
        {
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);

            return response.StatusCode switch
            {
                HttpStatusCode.InternalServerError => Result.Factory.AsError(
                    $"Internal server error {uri}. Reason: {response.ReasonPhrase}"
                ),
                HttpStatusCode.Forbidden => Result.Factory.AsError(
                    $"Access denied to {uri}."
                ),
                HttpStatusCode.NotFound => Result.Factory.AsError(
                    $"Resource {uri} not found."
                ),
                _ => Result.Factory.AsSuccess()
            };
        }
        
        private async Task<Result<TData>> GetResultFromRequest<TData>(
            string uri,
            HttpRequestMessage requestMessage,
            CancellationToken cancellationToken
        )
        {
            var response = await httpClient.SendAsync(requestMessage, cancellationToken);
                    
            return response.StatusCode switch
            {
                HttpStatusCode.InternalServerError => Result<TData>.Factory.AsError(
                    $"Internal server error {uri}. Reason: {response.ReasonPhrase}"
                ),
                HttpStatusCode.Forbidden => Result<TData>.Factory.AsError(
                    $"Access denied to {uri}."
                ),
                HttpStatusCode.NotFound => Result<TData>.Factory.AsError(
                    $"Resource {uri} not found."
                ),
                _ => await deserialize.GetResult<TData>(uri, response)
            };
        }

        #endregion
    }
}