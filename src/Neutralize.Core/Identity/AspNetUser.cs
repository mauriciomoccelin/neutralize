using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Neutralize.Identity
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string Name => accessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetUserClaims()
        {
            return accessor.HttpContext.User.Claims;
        }

        public Claim GetClaim(string type)
        {
            return GetUserClaims().First(claim => claim.Type == type);
        }

        public string GetUserEmail()
        {
            return IsAutenticated() ? accessor.HttpContext.User.GetUserEmail() : "";
        }

        public long GetUserId()
        {
            long.TryParse(accessor.HttpContext.User.GetUserId(), out var id);
            return IsAutenticated() ? id  : default;
        }

        public bool IsAutenticated()
        {
            return accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public HttpContext GetHttpContext()
        {
            return accessor.HttpContext;
        }
    }
}
