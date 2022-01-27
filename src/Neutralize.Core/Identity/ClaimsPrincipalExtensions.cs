using System;
using System.Linq;
using System.Security.Claims;

namespace Neutralize.Identity
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }

            var claim = principal.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }

            var claim = principal.Claims.Single(c => c.Type == ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
