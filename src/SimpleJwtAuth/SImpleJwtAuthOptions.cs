using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Http;

namespace SimpleJwtAuth
{
    public class SimpleJwtAuthOptions : AuthenticationOptions
    {
        public SimpleJwtAuthOptions() : base()
        {
            AuthenticationScheme = SimpleJwtAuthDefaults.AuthenticationScheme;
        }

        public string Secret { get; set; }

        public string Audience { get; set; } = "JWT Audience";

        public PathString TokenEndpoint { get; set; } = new PathString("/api/token");

        public TimeSpan ExpirationTimeout { get; set; } = TimeSpan.FromHours(1);
    }
}
