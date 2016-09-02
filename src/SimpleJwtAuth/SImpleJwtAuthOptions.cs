using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SimpleJwtAuth
{
    public class SimpleJwtAuthOptions : AuthenticationOptions
    {
        public SimpleJwtAuthOptions()
        {
            AuthenticationScheme = SimpleJwtAuthDefaults.AuthenticationScheme;
        }

        public string Secret { get; set; }

        public string Audience { get; set; } = "JWT Audience";

        public PathString TokenEndpoint { get; set; } = new PathString("/api/token");

        public TimeSpan ExpirationTimeout { get; set; } = TimeSpan.FromHours(1);
    }
}
