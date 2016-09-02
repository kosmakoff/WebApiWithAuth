using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SimpleJwtAuth
{
    public static class SimpleJwtAuthAppBuilderExtensions
    {
        public static IApplicationBuilder UseSimpleJwtAuth<TUser>(this IApplicationBuilder app, SimpleJwtAuthOptions options)
            where TUser : IdentityUser
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app
                .UseMiddleware<SimpleJwtAuthMiddleware<TUser>>(Options.Create(options))
                .UseMiddleware<SimpleJwtAuthTokenMiddleware<TUser>>(Options.Create(options));
        }
    }
}
