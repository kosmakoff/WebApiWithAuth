using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace WebApiWithAuth.Common
{
    public static class AuthServerScopes
    {
        public static Scope Api => new Scope
        {
            Name = "api",
            DisplayName = "My API",
            Type = ScopeType.Resource
        };
    }
}
