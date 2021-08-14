using Microsoft.Extensions.Hosting;

namespace Neutralize.Extensions
{
    public static class IHostEnvironmentExtensions
    {
        /// <summary>
        /// Returns true if the aspnet environment is running with "Tests"
        /// </summary>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static bool IsTests(this IHostEnvironment environment)
        {
            return environment.IsEnvironment("Tests");
        }
    }
}