using System;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace evolib.Log
{
	public static class LogExtensions
	{
		public static IWebHostBuilder UseEvoLog(this IWebHostBuilder builder)
		{
			return AspNetExtensions.UseNLog(builder);
		}
	}
}
