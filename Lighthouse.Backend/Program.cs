using Lighthouse.Backend.Services.Interfaces;
using Lighthouse.Backend.Services.Implementation;
using Lighthouse.Backend.Services.Factories;
using Lighthouse.Backend.Services.Implementation.Repositories;
using Microsoft.EntityFrameworkCore;
using Lighthouse.Backend.Models;
using Lighthouse.Backend.Factories;
using Lighthouse.Backend.Services.Implementation.BackgroundServices;
using Lighthouse.Backend.Data;
using System.Globalization;
using Lighthouse.Backend.Services.Implementation.WorkItemServices;
using Serilog;
using System.Text.Json.Serialization;
using Serilog.Settings.Configuration;
using Lighthouse.Backend.Models.History;
using Lighthouse.Backend.Models.Preview;
using Lighthouse.Backend.Services.Implementation.Update;
using Lighthouse.Backend.Services.Interfaces.Update;
using System.Collections.Concurrent;

namespace Lighthouse.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting up Lighthouse!");

            try
            {
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentCulture;

                var builder = WebApplication.CreateBuilder(args);

                var fileSystemService = new FileSystemService();
                var configFileUpdater = new ConfigFileUpdater(fileSystemService);
                var serilogConfiguration = new SerilogLogConfiguration(builder.Configuration, configFileUpdater, fileSystemService);
                builder.Services.AddSingleton<ILogConfiguration>(serilogConfiguration);

                builder.Services.AddSerilog((services, lc) => lc
                    .ReadFrom.Configuration(builder.Configuration, new ConfigurationReaderOptions(ConfigurationAssemblySource.AlwaysScanDllFiles))
                    .ReadFrom.Services(services)
                    .MinimumLevel.ControlledBy(serilogConfiguration.LoggingLevelSwitch)
                    .Enrich.FromLogContext());

                Log.Information("Setting Culture Info to {CultureName}", CultureInfo.CurrentCulture.Name);

                // Add services to the container.
                builder.Services.AddRazorPages();
                builder.Services.AddDbContext<LighthouseAppContext>(options =>
                    options.UseSqlite(
                        builder.Configuration.GetConnectionString("LighthouseAppContext") ?? throw new InvalidOperationException("Connection string 'LighthouseAppContext' not found"),
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                    )
                );

                // Repos
                builder.Services.AddScoped<IRepository<Team>, TeamRepository>();
                builder.Services.AddScoped<IRepository<Project>, ProjectRepository>();
                builder.Services.AddScoped<IRepository<Feature>, FeatureRepository>();
                builder.Services.AddScoped<IRepository<WorkTrackingSystemConnection>, WorkTrackingSystemConnectionRepository>();
                builder.Services.AddScoped<IRepository<AppSetting>, AppSettingRepository>();
                builder.Services.AddScoped<IRepository<FeatureHistoryEntry>, FeatureHistoryRepository>();
                builder.Services.AddScoped<IRepository<PreviewFeature>, PreviewFeatureRepository>();

                // Factories
                builder.Services.AddScoped<IWorkItemServiceFactory, WorkItemServiceFactory>();
                builder.Services.AddScoped<IIssueFactory, IssueFactory>();
                builder.Services.AddScoped<IWorkTrackingSystemFactory, WorkTrackingSystemFactory>();

                // Services
                builder.Services.AddScoped<IRandomNumberService, RandomNumberService>();
                builder.Services.AddScoped<IMonteCarloService, MonteCarloService>();
                builder.Services.AddScoped<ITeamUpdateService, TeamUpdateService>();
                builder.Services.AddScoped<IWorkItemCollectorService, WorkItemCollectorService>();
                builder.Services.AddScoped<ILexoRankService, LexoRankService>();
                builder.Services.AddScoped<IConfigFileUpdater, ConfigFileUpdater>();
                builder.Services.AddScoped<IFileSystemService, FileSystemService>();
                builder.Services.AddScoped<IAppSettingService, AppSettingService>();
                builder.Services.AddScoped<ILighthouseReleaseService, LighthouseReleaseService>();
                builder.Services.AddScoped<IAssemblyService, AssemblyService>();
                builder.Services.AddScoped<IFeatureHistoryService, FeatureHistoryService>();

                builder.Services.AddScoped<AzureDevOpsWorkItemService>();
                builder.Services.AddScoped<JiraWorkItemService>();

                // Background Services
                builder.Services.AddHostedService<TeamUpdateBackgroundService>();
                builder.Services.AddHostedService<FeatureUpdateService>();
                builder.Services.AddHostedService<ForecastUpdateService>();
                builder.Services.AddHostedService<DataRetentionService>();

                builder.Services.AddSingleton<ICryptoService, CryptoService>();
                builder.Services.AddSingleton<IGitHubService, GitHubService>();

                var updateStatuses = new ConcurrentDictionary<UpdateKey, UpdateStatus>();
                builder.Services.AddSingleton(updateStatuses);
                builder.Services.AddSingleton<IUpdateQueueService, UpdateQueueService>();

                builder.Services
                    .AddControllers().AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

                // Add Swagger services
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // Add SignalR
                builder.Services.AddSignalR()
                     .AddJsonProtocol(options =>
                     {
                         options.PayloadSerializerOptions.Converters
                            .Add(new JsonStringEnumConverter());
                     });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    app.UseHsts();

                    app.UseDefaultFiles();
                    app.UseStaticFiles();

                    app.UseSpa(spa =>
                    {
                        spa.Options.SourcePath = "wwwroot";
                        spa.Options.DefaultPage = "/index.html";
                    });
                }

                app.MapHub<UpdateNotificationHub>("api/updateNotificationHub");

                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Lighthouse API V1");
                    c.RoutePrefix = "api/swagger";
                });

                app.UseHttpsRedirection();

                app.UseRouting();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}