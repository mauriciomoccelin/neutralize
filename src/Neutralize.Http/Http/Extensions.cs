using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Neutralize.Http
{
    public static class Extensions
    {
        public static HttpRequestMessage CopyAuthorizationHeaderFrom(
            this HttpRequestMessage request,
            HttpContext context
        )
        {
            var authorizationHeader = context?.Request.Headers[nameof(Authorization)];
            
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add(
                    nameof(Authorization), new string[]
                    {
                        authorizationHeader.Value
                    }
                );
            }

            return request;
        }

        public static HttpRequestMessage Apply(
            this HttpRequestMessage request, 
            Authorization authorization
        )
        {
            authorization ??= Authorization.Empty;
            
            if (authorization.IsEmpty is false)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    authorization.Method,
                    authorization.Token
                );
            }
            
            return request;
        }

        public static string GetOriginFromUri(this Uri uri)
        {
            return $"{uri.Scheme}://{uri.DnsSafeHost}:{uri.Port}";
        }
    }
}
