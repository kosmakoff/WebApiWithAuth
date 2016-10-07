using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace WebApiWithAuth.Tools.Data
{
    public abstract class DbContextFactoryBase<T> : IDbContextFactory<T> where T : DbContext
    {
        private static string GetConnectionString(string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            var configuration = builder.Build();

            return configuration["Data:ConnectionString"];
        }

        public T Create(DbContextFactoryOptions options)
        {
            var connectionString = GetConnectionString(options.EnvironmentName);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("No configuration found");

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<T>();

            dbContextOptionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.MigrationsAssembly("WebApiWithAuth.Tools");
            });

            var constructorInfo = typeof(T).GetConstructor(new[] {typeof(DbContextOptions<T>)});
            var instance = constructorInfo.Invoke(new object[] {dbContextOptionsBuilder.Options});

            return (T) instance;
        }
    }
}
