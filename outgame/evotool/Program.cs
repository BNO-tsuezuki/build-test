﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using evolib.Log;

namespace evotool
{
    public class Program
    {
		public static void Main(string[] args)
		{
			Logger.Initialize();

			Logger.Logging(
				new LogObj().AddChild(new LogModels.ServerStart
				{
					Date = DateTime.UtcNow,
				})
			);

			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			//return WebHost.CreateDefaultBuilder(args)
			//         .UseStartup<Startup>()
			//         .Build();

			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory() + "/.." )
				.AddJsonFile($"appsettings.json", optional: false)
				.AddJsonFile($"appsettings.{environmentName}.json", optional: false)
				.AddJsonFile($"RdbConnections.json", optional: false)
				.AddJsonFile($"KvsConnections.json", optional: false)
				.AddJsonFile($"MatchingServer.json", optional: false)
				.AddJsonFile($"AuthenticationServer.json", optional: false)
				.AddJsonFile($"SequencingServer.json", optional: false)
				.AddJsonFile($"DeliveryData.json", optional: false)
				.AddJsonFile($"evotool/hosting.json", optional: false)
                .AddJsonFile($"evotool/hosting.{environmentName}.json", optional: false)
                .AddCommandLine(args)
				.Build();

			return
			WebHost.CreateDefaultBuilder(args)
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
