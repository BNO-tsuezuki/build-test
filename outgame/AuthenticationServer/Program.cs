using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticationServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//CreateWebHostBuilder(args).Build().Run();

			var webHost = BuildWebHost(args);

			webHost.Run();
		}

		//public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
		//	WebHost.CreateDefaultBuilder(args)
		//		.UseStartup<Startup>();
		public static IWebHost BuildWebHost(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory() + "/..")
				.AddJsonFile($"RdbConnections.json", optional: false)
				.AddJsonFile($"KvsConnections.json", optional: false)
				.AddJsonFile($"AuthenticationServer/hosting.json", optional: false)
				.AddCommandLine(args)
				.Build();

			return
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel(opt => opt.AddServerHeader = false)
				.UseConfiguration(config)
				.UseStartup<Startup>()
				.Build();
		}
	}
}
