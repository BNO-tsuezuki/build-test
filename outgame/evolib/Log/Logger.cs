using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;


namespace evolib.Log
{
	public static class Logger
	{
		static object LockObj = new object();

		static string RootLog = "{}";

		public static void Initialize()
		{
			var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			var factory = NLogBuilder.ConfigureNLog($"NLog.{environmentName}.config");

			RootLog = Newtonsoft.Json.JsonConvert.SerializeObject(new
			{
				AppName = AppDomain.CurrentDomain.FriendlyName,
				ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
				Host = System.Net.Dns.GetHostName(),
				Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
			});
		}

		public enum LogLevel
		{
			Trace,
			Debug,
			Info,
			Warn,
			Error,
			Fatal,
		}

		static NLog.Logger LoggerAll = NLog.LogManager.GetLogger("Evo.General");


		public static void Logging(ILogObj logObj, LogLevel level = LogLevel.Info)
		{
			lock (LockObj)
			{
				LoggerAll.SetProperty("Tag", $"App.{logObj.Tag()}");

				var log = RootLog.Insert(RootLog.LastIndexOf('}'), $",{logObj.Serialize()}");

				Exception ex = null;
				switch (level)
				{
					case LogLevel.Trace:
						LoggerAll.Trace(ex, log);
						break;
					case LogLevel.Debug:
						LoggerAll.Debug(ex, log);
						break;
					case LogLevel.Info:
						LoggerAll.Info(ex, log);
						break;
					case LogLevel.Warn:
						LoggerAll.Warn(ex, log);
						break;
					case LogLevel.Error:
						LoggerAll.Error(ex, log);
						break;
					case LogLevel.Fatal:
						LoggerAll.Fatal(ex, log);
						break;
					default:
						break;
				}
			}
		}
	}
}
