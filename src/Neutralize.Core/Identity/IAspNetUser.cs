﻿using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Neutralize.Identity
{
    public interface IAspNetUser
    {
        HttpContext GetHttpContext();
        string Name { get; }
        long GetUserId();
        string GetUserEmail();
        bool IsAutenticated();
        Claim GetClaim(string type);
        bool TryGetClaim(string type, out Claim claim);
        IEnumerable<Claim> GetUserClaims();
    }
}
