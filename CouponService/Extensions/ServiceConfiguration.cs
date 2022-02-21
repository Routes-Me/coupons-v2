using AdvertisementService.Models;
using CouponService.Abstraction;
using CouponService.Models.Base;
using CouponService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CouponService.Extensions
{
    public static class ServiceConfiguration
    {
        internal static void AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }
        internal static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorageBlobConfig>(configuration.GetSection("AzureStorageBlobConfig"));
            services.Configure<Dependencies>(configuration.GetSection("Dependencies"));
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
        }

        internal static void AddController(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }
        internal static void AddCronJob(this IServiceCollection services)
        {
            //services.AddCronJob<AnalyticSynced>(c =>
            //{
            //    c.TimeZoneInfo = TimeZoneInfo.Local;
            //    c.CronExpression = @"0 */4 * * *"; // Run every 4 hours
            //});
        }

        internal static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CouponContext>(options => options.UseMySql(configuration.GetConnectionString("DefaultConnection")));
        }

        internal static void AddInjections(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        internal static void Cors(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }
    }
}
