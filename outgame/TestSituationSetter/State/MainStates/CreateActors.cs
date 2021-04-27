using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class CreateActors : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				var login = new EvoApiRequester<
					evoapi.ProtocolModels.Auth.Login.Request,
					evoapi.ProtocolModels.Auth.Login.Response>();
				var setFirstOnetime = new EvoApiRequester<
					evoapi.ProtocolModels.PlayerInformation.SetFirstOnetime.Request,
					evoapi.ProtocolModels.PlayerInformation.SetFirstOnetime.Response>();

				foreach( var account in TestAccounts.Actors())
				{
					login.request.account = account.Account;
					login.request.password = account.Password;
					login.request.packageVersion = new int[] { 99, };

					var resLogin = await login.PostAsync();

					if (resLogin.StatusCode != System.Net.HttpStatusCode.OK
						|| resLogin.Payload.playerId == 0)
					{
						ConsoleWriter.Error($"Login is failed.");
						break;
					}

					if (0 != (resLogin.Payload.initialLevel
								& evolib.PlayerInformation.InitialLevelFlg.Name))
					{
						ConsoleWriter.Warning($"Already exists (playerId:{resLogin.Payload.playerId}).");
						continue;
					}

					setFirstOnetime.request.playerName = account.Name;
					var resSetFirstOnetime = await setFirstOnetime.PostAsync(
						$"{resLogin.Payload.token}"
					);

					if (resSetFirstOnetime.StatusCode != System.Net.HttpStatusCode.OK) continue;

					ConsoleWriter.Succeeded(
						$"{resLogin.Payload.playerId}:{resSetFirstOnetime.Payload.initialLevel}"
					);
				}

				StateMachine.SendEvent(Main.EventTrigger.Next);
			});
		}
	}
}
