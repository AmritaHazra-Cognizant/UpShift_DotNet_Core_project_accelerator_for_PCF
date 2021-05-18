using DotNetCore.API.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Web
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            try
            {
                Log.Logger.Debug("Application started...");
                CreateWebHostBuilder(args, config).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration config) =>
            WebHost.CreateDefaultBuilder(args).UseConfiguration(config)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);

                }).UseStartup<Startup>().
            UseDefaultServiceProvider(opt => opt.ValidateScopes = false)
            .UseNLog();
    }
}
