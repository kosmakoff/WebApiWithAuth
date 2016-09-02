using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiWithAuth.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SimpleJwtAuth;

namespace WebApiWithAuth
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
                .AddUserSecrets()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:ConnectionString"]));

            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.Lockout.AllowedForNewUsers = false;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
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

            //if (env.IsProduction())
            //{
            //    var ksi = app.ServerFeatures.Get<IKestrelServerInformation>();
            //    ksi.NoDelay = true;
            //    app.UseKestrelHttps(new X509Certificate2(@"c:\Dev\CA\web.local.pfx"));
            //}

            // app.UseStaticFiles();

            app.UseIdentity();

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = Configuration["Auth:Google:ClientId"],
                ClientSecret = Configuration["Auth:Google:Secret"]
            });

            app.UseSimpleJwtAuth<ApplicationUser>(new SimpleJwtAuthOptions
            {
                Audience = Configuration["Auth:JWT:Audience"] ?? "Sample Audience",
                ClaimsIssuer = Configuration["Auth:JWT:issuer"] ?? "Sample Issuer",

                Secret = Configuration["Auth:JWT:secret"] ?? "SECRET",

                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder
                .MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}