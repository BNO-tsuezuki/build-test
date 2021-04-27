using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace StressTest.Protocols
{
	public class HttpRequester<Req, Res> : evolib.Util.HttpRequester<Req, Res> where Req : new()
	{
		static HttpClient _httpClient = new HttpClient();

		public override string Path
		{
			get
			{
				var splits = request.GetType().ToString().Split('.', '+');
				return ("/api/" + splits[splits.Length - 3] + "/" + splits[splits.Length - 2]);
			}
		}

		protected override HttpClient HttpClient
		{
			get
			{
				return _httpClient;
			}
		}
	}


	class Login : HttpRequester<
		evoapi.ProtocolModels.Auth.Login.Request,
		evoapi.ProtocolModels.Auth.Login.Response
	>
	{ }
	class SetFirstOnetime : HttpRequester<
		evoapi.ProtocolModels.PlayerInformation.SetFirstOnetime.Request,
		evoapi.ProtocolModels.PlayerInformation.SetFirstOnetime.Response
	>
	{ }


	class HandShakeResponse
	{
		public string pushCode { get; set; }
		public int nextResponseSeconds { get; set; }
	}
	class HandShack : HttpRequester<
		evoapi.ProtocolModels.HandShake.HandShake.Request,
		HandShakeResponse
	>
	{
		public override string Path
		{
			get
			{
				var splits = request.GetType().ToString().Split('.', '+');
				return ("/api/" + splits[splits.Length - 3]);
			}
		}
	}

	class MasterDataGet: HttpRequester<
		evoapi.ProtocolModels.MasterData.Get.Request,
		evoapi.ProtocolModels.MasterData.Get.Response
	>
	{ }

	class GetFriends : HttpRequester<
		evoapi.ProtocolModels.Social.GetFriends.Request,
		evoapi.ProtocolModels.Social.GetFriends.Response
	>
	{ }


	class EntryPlayer : HttpRequester<
		evoapi.ProtocolModels.Matching.EntryPlayer.Request,
		evoapi.ProtocolModels.Matching.EntryPlayer.Response
	>
	{ }
	
}
