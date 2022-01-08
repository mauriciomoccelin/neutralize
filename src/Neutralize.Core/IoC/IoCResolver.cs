using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Neutralize.IoC
{
    public static class IoCResolver
    {
        private static IServiceProvider Provider { get; set; }
        public static void Inicialize(IServiceProvider provider) => Provider = provider;

        public static async Task<TResult> ServiceWrapper<TService, TResult>(Func<TService, Task<TResult>> func)
        {
            using var scope = Provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();
            var result = await func.Invoke(service);
            
            scope.Dispose();
            
            return result;
        }

        public static TResult ServiceWrapper<TService, TResult>(Func<TService, TResult> func)
        {
            using var scope = Provider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();
            var result = func.Invoke(service);

            scope.Dispose();
            
            return result;
        }
    }
}