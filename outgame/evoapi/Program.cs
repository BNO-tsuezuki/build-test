using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using evolib.Services.MasterData;
using evolib.FamilyServerInfo;
using evolib.Log;

namespace evoapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
			ThreadPool.SetMinThreads(128, 128);

			Logger.Initialize();

			Logger.Logging(
				new LogObj().AddChild(new LogModels.ServerStart
				{
					Date = DateTime.UtcNow,
				})
			);

			var webHost = BuildWebHost(args);

			while( MasterDataLoader.LatestMasterData == null || !MatchingServerInfo.Ready() )
			{
				Thread.Sleep(1000);
			}

			webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
			//return WebHost.CreateDefaultBuilder(args)
			//         .UseStartup<Startup>()
			//         .Build();

			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/..")
				.AddJsonFile($"appsettings.json", optional: false)
				.AddJsonFile($"appsettings.{environmentName}.json", optional: false)
				.AddJsonFile($"RdbConnections.json", optional: false)
				.AddJsonFile($"KvsConnections.json", optional: false)
				.AddJsonFile($"MatchingServer.json", optional: false)
				.AddJsonFile($"AuthenticationServer.json", optional: false)
				.AddJsonFile($"SequencingServer.json", optional: false)
				.AddJsonFile($"DeliveryData.json", optional: false)
				.AddJsonFile($"systeminfo.json", optional: false)
				.AddJsonFile($"evoapi/hosting.json", optional: false)
                .AddJsonFile($"evoapi/hosting.{environmentName}.json", optional: false)
                .AddCommandLine(args)
                .Build();

            return
            WebHost.CreateDefaultBuilder(args)
				.UseKestrel(opt=>opt.AddServerHeader = false)
                .UseConfiguration(config)
                .UseStartup<Startup>()
				//.ConfigureLogging(logging =>
				//{
				//	logging.ClearProviders();
				//	logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
				//})
				//.UseEvoLog()
				.Build();
        }
    }
}
