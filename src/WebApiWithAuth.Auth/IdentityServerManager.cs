using System.Collections.Generic;
using IdentityServer4.Models;
using WebApiWithAuth.Common;

namespace WebApiWithAuth.Auth
{
    public static class IdentityServerManager
    {
        public static IEnumerable<Client> GetClients()
        {
            yield return AuthServerClients.Mvc;
            yield return AuthServerClients.Api;
        }

        public static IEnumerable<Scope> GetScopes()
        {
            yield return StandardScopes.OpenId;
            yield return StandardScopes.Profile;
            yield return StandardScopes.OfflineAccess;
            yield return AuthServerScopes.Api;
        }
    }
}
