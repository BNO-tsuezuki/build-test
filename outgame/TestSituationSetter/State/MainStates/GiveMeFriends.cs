using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class GiveMeFriends : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		class HandShakeResponse : evoapi.ProtocolModels.HandShake.HandShake.ResponseBase {}

		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				var login = new EvoApiRequester<
						   evoapi.ProtocolModels.Auth.Login.Request,
						   evoapi.ProtocolModels.Auth.Login.Response>();

				var alreadyFriends = new Dictionary<long, int>();
				var alreadyRequests = new Dictionary<long, int>();

				{
					login.request.account = GlobalSettings.MyAccount;
					login.request.password = GlobalSettings.MyPassword;
					login.request.packageVersion = new int[] { 99, };
					var resLogin = await login.PostAsync();

					if (resLogin.StatusCode != System.Net.HttpStatusCode.OK)
					{
						return;
					}

					var getFriends = new EvoApiRequester<
					evoapi.ProtocolModels.Social.GetFriends.Request,
					evoapi.ProtocolModels.Social.GetFriends.Response>();
					var resGetFriends = await getFriends.PostAsync(
						$"{resLogin.Payload.token}"
					);

					resGetFriends.Payload.lists.Friends.ForEach(
						i => alreadyFriends[i.playerId] = 0
					);
					resGetFriends.Payload.lists.Requests.ForEach(
						i=> alreadyRequests[i.playerId] = 0
					);
				}


				var friendRequest = new EvoApiRequester<
					evoapi.ProtocolModels.Social.SendFriendRequest.Request,
					evoapi.ProtocolModels.Social.SendFriendRequest.Response>();
				friendRequest.request.playerId = GlobalSettings.MyPlayerId;
				foreach (var account in TestAccounts.Actors())
				{
					if (evolib.Social.MaxFriendRequestsCnt <= alreadyRequests.Count)
					{
						ConsoleWriter.Succeeded($"Reached limit requests.");
						break;
					}

					login.request.account = account.Account;
					login.request.password = account.Password;
					login.request.packageVersion = new int[] { 99, };
					var resLogin = await login.PostAsync();

					ConsoleWriter.Action($"Try ({account.Account}).");

					if (resLogin.StatusCode != System.Net.HttpStatusCode.OK
						|| resLogin.Payload.playerId == 0)
					{
						ConsoleWriter.Error($"Login is failed.");
						break;
					}

					if (0 == (resLogin.Payload.initialLevel
								& evolib.PlayerInformation.InitialLevelFlg.Name))
					{
						ConsoleWriter.Error("Player has no name.");
						break;
					}

					if (alreadyFriends.ContainsKey(resLogin.Payload.playerId))
					{
						ConsoleWriter.Warning($"Already friend.");
						continue;
					}

					if (alreadyRequests.ContainsKey(resLogin.Payload.playerId))
					{
						ConsoleWriter.Warning($"Already request.");
						continue;
					}

					var resFriendRequest = await friendRequest.PostAsync(
						$"{resLogin.Payload.token}"
					);

					if (resFriendRequest.StatusCode != System.Net.HttpStatusCode.OK
						|| !resFriendRequest.Payload.ok)
					{
						ConsoleWriter.Warning($"Request is Failed.");
						continue;
					}

					alreadyRequests[resLogin.Payload.playerId] = 123;

					ConsoleWriter.Succeeded(
						$"request from player:{resLogin.Payload.playerId}"
					);
				}

				StateMachine.SendEvent(Main.EventTrigger.Next);
			});
		}
	}
}
