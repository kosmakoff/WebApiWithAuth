using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApiWithAuth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json");

            var configuration = configBuilder.Build();

            var hostBuilder = new WebHostBuilder();

            // set urls and environment
            hostBuilder
                .UseUrls(configuration["urls"].Split(','))
                .UseEnvironment(configuration["environment"]);

            // set other common things
            hostBuilder
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            var host = hostBuilder.Build();

            host.Run();
        }
    }
}
