using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class MyAccount : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				ConsoleWriter.Prompt("Input [your account] > ");
				var account = await InputPrompt.Create();


				ConsoleWriter.Prompt("Input [your password] > ");
				var password = await InputPassPrompt.Create();


				ConsoleWriter.Action("Login...");

				var login = new EvoApiRequester<
					evoapi.ProtocolModels.Auth.Login.Request,
					evoapi.ProtocolModels.Auth.Login.Response>();

				login.request.account = account;
				login.request.password = password;
				login.request.packageVersion = new int[] { 99, };
				var resLogin = await login.PostAsync();

				if(resLogin.StatusCode != System.Net.HttpStatusCode.OK
					|| resLogin.Payload.playerId == 0 )
				{
					ConsoleWriter.Error("Login is failed.");
					StateMachine.SendEvent(Main.EventTrigger.Continue);
					return;
				}

				if(0 == (resLogin.Payload.initialLevel
								& evolib.PlayerInformation.InitialLevelFlg.Name))
				{
					ConsoleWriter.Error("Player has no name.");
					StateMachine.SendEvent(Main.EventTrigger.Continue);
					return;
				}
							
				GlobalSettings.MyAccount = account;
				GlobalSettings.MyPassword = password;
				GlobalSettings.MyPlayerId = resLogin.Payload.playerId;
				StateMachine.SendEvent(Main.EventTrigger.Next);
				ConsoleWriter.Succeeded("ok.");
			});
		}
	}
}
