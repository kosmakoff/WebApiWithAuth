using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;
using WebApiWithAuth.Models;
using SimpleJwtAuth;
using Microsoft.AspNet.Authentication.Cookies;
using System.Threading.Tasks;

namespace WebApiWithAuth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonLetterOrDigit = false;
                options.Password.RequireUppercase = false;

                options.Lockout.AllowedForNewUsers = false;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                /*
                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult<object>(null);
                    }
                };
                */
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<MyUserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, string>>()
                .AddRoleStore<MyRoleStore<ApplicationRole, ApplicationDbContext, string>>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor contextAccessor)
        {
            loggerFactory.MinimumLevel = LogLevel.Debug;
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // app.UseStaticFiles();

            app.UseIdentity();

            /*
            app.UseGoogleAuthentication(options =>
            {
                options.ClientId = Configuration["google:clientId"];
                options.ClientSecret = Configuration["google:secret"];
            });
            */

            app.UseSimpleJwtAuth<ApplicationUser>(options =>
            {
                options.Audience = Configuration["jwt:audience"] ?? "Sample Audience";
                options.ClaimsIssuer = Configuration["jwt:issuer"] ?? "Sample Issuer";

                options.Secret = Configuration["jwt:secret"] ?? "SECRET";

                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
