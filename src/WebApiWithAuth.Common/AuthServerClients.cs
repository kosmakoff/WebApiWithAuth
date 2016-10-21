using System.Collections.Generic;
using IdentityServer4.Models;

namespace WebApiWithAuth.Common
{
    public static class AuthServerClients
    {
        public static Client Api => new Client
        {
            ClientId = "api",
            ClientName = "API Client",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            RequireConsent = false,

            ClientSecrets = new List<Secret>
            {
                new Secret("api-secret".Sha256())
            },

            AllowedScopes = new List<string>
            {
                AuthServerScopes.Api.Name
            }
        };

        public static Client Mvc => new Client
        {
            ClientId = "mvc",
            ClientName = "MVC Client",
            AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            RedirectUris = new List<string>
            {
                // todo: fix hardcoded values
                "http://web.local.com:9083/signin-oidc"
            },

            PostLogoutRedirectUris = new List<string>
            {
                // todo: fix hardcoded values
                "http://web.local.com:9083"
            },

            AllowedScopes = new List<string>
            {
                StandardScopes.OpenId.Name,
                StandardScopes.Profile.Name,
                StandardScopes.OfflineAccess.Name,
                AuthServerScopes.Api.Name
            }
        };
    }
}
