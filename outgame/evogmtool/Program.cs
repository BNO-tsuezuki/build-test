using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;
using Microsoft.Extensions.Logging;

namespace evogmtool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // todo: NLog internal log
            // todo: NLog config ?
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            NLogBuilder.ConfigureNLog($"NLog.{environmentName}.config").GetCurrentClassLogger();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/..")
                .AddJsonFile($"appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false)
                .AddJsonFile($"RdbConnections.json", optional: false)
                .AddJsonFile($"ToolServer.json", optional: false)
				.AddJsonFile($"evogmtool/hosting.json", optional: false)
				.AddJsonFile($"evogmtool/hosting.{environmentName}.json", optional: false)
				.Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    // todo: 要確認
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
                .Build();
        }
    }
}
