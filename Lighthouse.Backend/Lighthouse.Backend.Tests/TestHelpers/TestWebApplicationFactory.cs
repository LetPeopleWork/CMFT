﻿using Lighthouse.Backend.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

namespace Lighthouse.Backend.Tests.TestHelpers
{
    public sealed class TestWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        private readonly string DatabaseFileName;

        public TestWebApplicationFactory()
        {
            DatabaseFileName = $"IntegrationTests_{Path.GetRandomFileName().Replace(".", "")}.db";
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveServices(services);

                services.AddDbContext<LighthouseAppContext>(options =>
                {
                    options.UseSqlite($"DataSource={DatabaseFileName}",
                        x => x.MigrationsAssembly("Lighthouse.Migrations.Sqlite"));
                });
            });
        }

        private void RemoveServices(IServiceCollection services)
        {
            RemoveAllDbContextFromServices(services);
            RemoveHostedServices(services);
        }

        private void RemoveHostedServices(IServiceCollection services)
        {
            services.RemoveAll<IHostedService>();
        }

        private void RemoveAllDbContextFromServices(IServiceCollection services)
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<LighthouseAppContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

            if (dbConnectionDescriptor != null)
            {
                services.Remove(dbConnectionDescriptor);
            }
        }

        private void DeleteDatabaseFile()
        {
            if (File.Exists(DatabaseFileName))
            {
                File.Delete(DatabaseFileName);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DeleteDatabaseFile();
        }
    }
}
