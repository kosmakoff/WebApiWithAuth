using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity.EntityFramework;

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
                .UseMiddleware<SimpleJwtAuthMiddleware<TUser>>(options)
                .UseMiddleware<SimpleJwtAuthTokenMiddleware<TUser>>(options);
        }

        public static IApplicationBuilder UseSimpleJwtAuth<TUser>(this IApplicationBuilder app, Action<SimpleJwtAuthOptions> configureOptions)
            where TUser : IdentityUser
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = new SimpleJwtAuthOptions();
            if (configureOptions != null)
            {
                configureOptions(options);
            }
            return app.UseSimpleJwtAuth<TUser>(options);
        }
    }
}
