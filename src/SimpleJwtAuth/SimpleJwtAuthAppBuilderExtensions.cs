using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SimpleJwtAuth
{
    public static class SimpleJwtAuthAppBuilderExtensions
    {
        public static IApplicationBuilder UseSimpleJwtAuth<T>(this IApplicationBuilder app, SimpleJwtAuthOptions options)
            where T : IdentityUser
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
                .UseMiddleware<SimpleJwtAuthMiddleware>(options)
                .UseMiddleware<SimpleJwtAuthTokenMiddleware<T>>(options);
        }

        public static IApplicationBuilder UseSimpleJwtAuth<T>(this IApplicationBuilder app, Action<SimpleJwtAuthOptions> configureOptions)
            where T : IdentityUser
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
            return app.UseSimpleJwtAuth<T>(options);
        }
    }
}
