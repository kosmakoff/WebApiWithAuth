using System.Collections.Generic;
using IdentityServer4.Models;

namespace WebApiWithAuth.Auth
{
    public static class IdentityServerManager
    {
        public static IEnumerable<Client> GetClients()
        {
            // MVC
            yield return new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Implicit,

                RequireConsent = true,

                ClientSecrets = new List<Secret>
                {
                    new Secret("mvc-secret".Sha256())
                },

                RedirectUris = new List<string>
                {
                    "http://127.0.0.1:9080/signin-oidc"
                },

                PostLogoutRedirectUris = new List<string>
                {
                    "http://127.0.0.1:9080"
                },

                AllowedScopes = new List<string>
                {
                    StandardScopes.OpenId.Name,
                    StandardScopes.Profile.Name,
                    StandardScopes.OfflineAccess.Name
                }
            };

            // API
            yield return new Client
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
                    "api1"
                }
            };
        }

        public static IEnumerable<Scope> GetScopes()
        {
            yield return StandardScopes.OpenId;
            yield return StandardScopes.Profile;
            yield return StandardScopes.OfflineAccess;
            yield return new Scope
            {
                Name = "api1",
                DisplayName = "My API"
            };
        }
    }
}
