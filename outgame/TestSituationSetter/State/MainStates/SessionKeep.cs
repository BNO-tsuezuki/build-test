using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using evoapi.ProtocolModels.Test;

namespace TestSituationSetter.State.MainStates
{
	public class SessionKeep : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				ConsoleWriter.Prompt("Input [account type] > ");
				var accounttype = await InputPrompt.Create();

				ConsoleWriter.Prompt("Input [account] > ");
				var account = await InputPrompt.Create();

				ConsoleWriter.Prompt("Input [password] > ");
				var password = await InputPassPrompt.Create();


				var accountType = evolib.Account.Type.Dev1;
				var authToken = "";
				if (accounttype == "inky")
				{
					accountType = evolib.Account.Type.Inky;
					var accessToken = "";

					{
						var requester = new evolib.Multiplatform.Inky.Login3();
						requester.request.email = account;
						requester.request.password = password;
						var response = await requester.PostAsync();
						if( response.StatusCode == System.Net.HttpStatusCode.OK)
						{
							accessToken = response.Payload.data.access_token;
						}
					}
					if(!string.IsNullOrEmpty(accessToken))
					{
						var requester = new evolib.Multiplatform.Inky.LoginTemp();
						var response = await requester.PostAsync(accessToken);
						if (response.StatusCode == System.Net.HttpStatusCode.OK)
						{
							authToken = response.Payload.data.access_token;
						}
					}
				}

				var handShakeKeeper
					= await HandShakeKeeper.CreateAsync(account, password,
															authToken, accountType);

				if (handShakeKeeper != null)
				{
					ConsoleWriter.Action("session keepon.");
					ConsoleWriter.Info($"Bearer {handShakeKeeper.Token}");
					ConsoleWriter.Prompt("<Press [Enter] key to exit>");
					await InputPrompt.Create();

					await handShakeKeeper.CancelAsync();
				}


				StateMachine.SendEvent(Main.EventTrigger.Next);
			});
		}
	}
}
