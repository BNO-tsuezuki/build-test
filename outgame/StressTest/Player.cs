using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using StressTest.Protocols;

namespace StressTest
{
    public class Player
    {
		public Task Start(int playerIndex)
		{
			return Task.Run(async () =>
			{
				var httpClientMain = new HttpClient();

				var reqLogin = new Login();
				reqLogin.request.account = $"LoadTester{playerIndex}@BNO";
				//reqLogin.request.password = "";
				var resLogin = await reqLogin.PostAsyncXXX(Global.ApiServerUri);

				if (resLogin.StatusCode != System.Net.HttpStatusCode.OK || string.IsNullOrEmpty(resLogin.Payload.token))
				{
					Console.WriteLine(playerIndex + " : Login Error");
					return;
				}

				Console.WriteLine(playerIndex + " : Login");

				if (resLogin.Payload.initialLevel ==  0)
				{
					var reqSetFirstOnetime = new SetFirstOnetime();
					reqSetFirstOnetime.request.playerName = $"LoadTester{playerIndex}";
					var resSetFirstOnetime = await reqSetFirstOnetime.PostAsyncXXX(Global.ApiServerUri, resLogin.Payload.token);
				}

				var keep = true;
				var b =
				Task.Run(async () =>
				{
					var httpClient = new HttpClient();

					while (true)
					{
						var reqHandShake = new HandShack();
						var resHandShake = await reqHandShake.PostAsyncXXX(Global.ApiServerUri, resLogin.Payload.token);

						//Console.WriteLine(reqLogin.request.password + ":PushCode=\"" + resHandShake.pushCode + "\"");
						//Console.WriteLine(resHandShake.pushCode);

						if(	resHandShake.StatusCode != System.Net.HttpStatusCode.OK ||
							resHandShake.Payload.pushCode == new evoapi.ProtocolModels.HandShake.Close.Response().pushCode ||
							resHandShake.Payload.pushCode == new evoapi.ProtocolModels.HandShake.Unauthorized.Response().pushCode )
						{
							Console.WriteLine(playerIndex + ":" + resHandShake.Payload.pushCode);
							keep = false;
							break;
						}
					}
				});


				var reqMasterDataGet = new MasterDataGet();
				var resMasterDataGet = await reqMasterDataGet.PostAsyncXXX(Global.ApiServerUri, resLogin.Payload.token);

				var reqGetFriends = new GetFriends();
				var resGetFriends = await reqGetFriends.PostAsyncXXX(Global.ApiServerUri, resLogin.Payload.token);

				//var reqEntryPlayer = new EntryPlayer();
				//reqEntryPlayer.request.matchType = evolib.Battle.MatchType.Casual;
				//var resEntryPlayer = await reqEntryPlayer.PostAsync(httpClientMain, resLogin.token);


				while (keep)
				{
					await Task.Delay(1000);
				}



			});
		}
    }
}
