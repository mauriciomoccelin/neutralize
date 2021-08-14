using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Neutralize.Identity
{
    public interface IAspNetUser
    {
        string Name { get; }
        long GetUserId();
        string GetUserEmail();
        bool IsAutenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetUserClaims();
        HttpContext GetHttpContext();
    }
}
