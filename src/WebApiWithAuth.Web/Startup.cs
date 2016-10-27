using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiWithAuth.Common;
using WebApiWithAuth.Web.Configuration;
using WebApiWithAuth.Web.Utils;

namespace WebApiWithAuth.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName.ToLower()}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }
        private IRouter _router;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ServersOptions>(Configuration.GetSection("Servers"));

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<ServersOptions> serverOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                AuthenticationScheme = "oidc",
                SignInScheme = "Cookies",

                Authority = serverOptions.Value.AuthServer,
                RequireHttpsMetadata = false,

                ClientId = AuthServerClients.Mvc.ClientId,
                ClientSecret = "secret",

                ResponseType = "code id_token",
                Scope = { AuthServerScopes.Api.Name, StandardScopes.OfflineAccess.Name},

                GetClaimsFromUserInfoEndpoint = true,
                SaveTokens = true,

                Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = context =>
                    {
                        var subClaim = context.Ticket.Principal.FindFirst(IdentityModel.JwtClaimTypes.Subject);

                        var nameClaim = new Claim(IdentityModel.JwtClaimTypes.Name, serverOptions.Value.AuthServer + "/" + subClaim.Value);

                        var givenNameClaim = context.Ticket.Principal.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

                        var familyNameClaim = context.Ticket.Principal.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

                        var newClaimsIdentity = new ClaimsIdentity(
                            context.Ticket.AuthenticationScheme, IdentityModel.JwtClaimTypes.Name, IdentityModel.JwtClaimTypes.Role);

                        if (nameClaim != null)
                            newClaimsIdentity.AddClaim(nameClaim);

                        if (givenNameClaim != null)
                            newClaimsIdentity.AddClaim(givenNameClaim);

                        if (familyNameClaim != null)
                            newClaimsIdentity.AddClaim(familyNameClaim);

                        context.Ticket = new AuthenticationTicket(new ClaimsPrincipal(newClaimsIdentity), context.Properties, context.Ticket.AuthenticationScheme);

                        return Task.CompletedTask;
                    },

                    OnRemoteFailure = context =>
                    {
                        if (context.Request.Form["error"] == "access_denied")
                        {
                            var dictionary = new RouteValueDictionary
                            {
                                ["controller"] = "Home",
                                ["action"] = "Index",
                                ["shit"] = "happened"
                            };

                            var vpc = new VirtualPathContext(context.HttpContext, null, dictionary);
                            var path = _router.GetVirtualPath(vpc).VirtualPath;

                            context.HandleResponse();
                            context.Response.Redirect(path);
                        }

                        return Task.CompletedTask;
                    }
                }
            });

            // custom UseMvc method, which returns router
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }, out _router);
        }
    }
}