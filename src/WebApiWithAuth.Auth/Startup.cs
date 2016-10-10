using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApiWithAuth.Data.Entities;

namespace WebApiWithAuth.Auth
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("hosting.json")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName.ToLower()}.json", optional: true)
                .AddUserSecrets()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Data:ConnectionString"];

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

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

            services.AddMvc();

            services.AddIdentityServer(SetIdentityServerOptions)
                .SetSigningCredential(GetSigningCertificate())
                .AddOperationalStore(options => options.UseSqlServer(connectionString))
                .AddConfigurationStore(options => options.UseSqlServer(connectionString))
                .AddInMemoryClients(IdentityServerManager.GetClients())
                .AddInMemoryScopes(IdentityServerManager.GetScopes())
                .AddAspNetIdentity<ApplicationUser>();
        }

        private X509Certificate2 GetSigningCertificate()
        {
            var certificateBase64 = Configuration["IdentityServer:SigningCertificate"];

            if (string.IsNullOrWhiteSpace(certificateBase64))
                throw new Exception("Signing certificate missing");

            var certificateBinary = Convert.FromBase64String(certificateBase64);
            var certificatePassword = Configuration["IdentityServer:SigningCertificatePassword"];

            if (string.IsNullOrWhiteSpace(certificatePassword))
                throw new Exception("Signing certificate password missing");

            var certificate = new X509Certificate2(certificateBinary, certificatePassword);
            return certificate;
        }

        private void SetIdentityServerOptions(IdentityServerOptions options)
        {
            var urls = Configuration["urls"];
            options.IssuerUri = urls.Split(',').First();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = Configuration["Auth:Google:ClientId"],
                ClientSecret = Configuration["Auth:Google:Secret"],

                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme
            });

            app.UseIdentity();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
