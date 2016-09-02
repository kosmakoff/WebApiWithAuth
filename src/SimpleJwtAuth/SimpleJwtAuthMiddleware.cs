using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;

namespace SimpleJwtAuth
{
    public class SimpleJwtAuthMiddleware<TUser> : AuthenticationMiddleware<SimpleJwtAuthOptions>
        where TUser : IdentityUser
    {
        public SimpleJwtAuthMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            IOptions<SimpleJwtAuthOptions> options)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(Options.Secret))
            {
                throw new InvalidOperationException("Secret must be set for JWT");
            }
        }

        protected override AuthenticationHandler<SimpleJwtAuthOptions> CreateHandler()
        {
            return new SimpleJwtAuthHandler<TUser>();
        }
    }
}
