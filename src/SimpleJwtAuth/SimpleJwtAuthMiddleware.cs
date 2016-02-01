using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using System;

namespace SimpleJwtAuth
{
    public class SimpleJwtAuthMiddleware<TUser> : AuthenticationMiddleware<SimpleJwtAuthOptions>
        where TUser : IdentityUser
    {
        public SimpleJwtAuthMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
            SimpleJwtAuthOptions options)
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

            if (string.IsNullOrEmpty(options.Secret))
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
