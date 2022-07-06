using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace CouponService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string standardVersion = "Standard version: " + "{0}.{1}.{2}";
            var standard = new Version(1, 0, 0);
            Console.WriteLine(standardVersion, standard.Major, standard.Minor, standard.Build);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
            })
                .UseStartup<Startup>();
        }
    }
}
