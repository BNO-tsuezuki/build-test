using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.Extensions.Configuration;

using evolib.Log;


namespace evolib.FamilyServerInfo
{
    public static class MatchingServerInfo
	{
		class ServerInfo
		{
			public bool Available { get; set; }
			public MatchingArea MatchingArea { get; set; }
			public FamilyServerInfo Info { get; set; }
		}
		static List<ServerInfo> _infos;

		public static void Initialize(IConfiguration configuration)
		{
			_infos = new List<ServerInfo>();
			foreach ( var cfg in configuration.GetSection("MatchingServers").GetChildren())
			{
				var info = new ServerInfo
				{
					Available = false,
					MatchingArea = MatchingArea.Unknown,
					Info = new FamilyServerInfo(),
				};

				info.Info.Initialize(cfg);

				_infos.Add(info);
			}

		}

		public static string AreaUri(MatchingArea area)
		{
			var info = _infos.Find(i => i.MatchingArea == area);

			if (info == null)
				throw new Exception($"{area.ToString()} is not found.");

			if (!info.Available )
				throw new Exception($"{area.ToString()} is not ready.");

			return info.Info.Uri;
		}

		public static bool Ready()
		{
			return (_infos.FindIndex(i => i.Available == false) < 0);
		}


		abstract class SetupRequester : Util.HttpRequester
		{
			static HttpClient _httpClient = new HttpClient();
			protected override HttpClient HttpClient { get { return _httpClient; } }
		}


		class SetRequester : SetupRequester
		{
			public MatchingArea MatchingArea { get; set; }

			public override string Path { get { return $"/api/Setup/Set?matchingarea={MatchingArea}"; } }
		}
		public static async Task<bool> SetupMultiMatchingServersAsync()
		{
			var index = 0;
			foreach (var area in Enum.GetValues(typeof(MatchingArea)))
			{
				if (_infos.Count <= index) break;

				var info = _infos[index];
				index++;

				if (info.Available) continue;

				info.MatchingArea = (MatchingArea)area;
				var result = "Failed";
				try
				{
					var requester = new SetRequester { MatchingArea = info.MatchingArea };
					var response = await requester.GetAsync(info.Info.Uri);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						info.Available = true;
						result = "Succeeded";
					}
				}
				catch(Exception)
				{
				}

				Logger.Logging(
					new LogObj().AddChild(new LogModels.InfoReport
					{
						Msg = $"{result} to setup matching server({info.MatchingArea}:{info.Info.Uri}).",
					})
				);
			}

			return Ready();
		}


		class GetRequester : SetupRequester
		{
			public override string Path { get { return $"/api/Setup/Get"; } }
		}
		public static async Task MultiMatchingServersAsync()
		{
			foreach( var info in _infos )
			{
				try
				{
					var requester = new GetRequester();
					var response = await requester.GetAsync(info.Info.Uri);
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						info.MatchingArea = Enum.Parse<MatchingArea>(response.Payload);
						info.Available = true;
					}
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
