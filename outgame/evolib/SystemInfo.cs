using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;

namespace evolib
{
	public static class SystemInfo
	{
		public static string GitCommitterDate { get; private set; }

		public static string InkyUrl { get; private set; }
		public static Dictionary<string,string> InkyApiHeader { get; private set; }

		public static string BattleServerPassword { get; private set; }

		public static void Initialize(IConfiguration configuration)
		{
			GitCommitterDate = configuration["git:committer_date"];

			InkyUrl = configuration["inky:url"];

			InkyApiHeader = new Dictionary<string, string>()
			{
				{ configuration["inky:apikey_name"], configuration["inky:apikey_value"] },
				{ configuration["inky:apisecretkey_name"], configuration["inky:apisecretkey_value"] },
			};

			BattleServerPassword = configuration["battleserver:password"];
		}
	}
}
