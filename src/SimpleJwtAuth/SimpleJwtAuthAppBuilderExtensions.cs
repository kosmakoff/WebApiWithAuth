using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;

namespace SimpleJwtAuth
{
    public static class SimpleJwtAuthAppBuilderExtensions
    {
        public static IApplicationBuilder UseSimpleJwtAuth(this IApplicationBuilder app, SimpleJwtAuthOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<SimpleJwtAuthMiddleware>(options);
        }

        public static IApplicationBuilder UseSimpleJwtAuth(this IApplicationBuilder app, Action<SimpleJwtAuthOptions> configureOptions)
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
            return app.UseSimpleJwtAuth(options);
        }
    }
}
