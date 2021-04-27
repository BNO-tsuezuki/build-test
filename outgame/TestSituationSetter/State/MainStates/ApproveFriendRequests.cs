using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class ApproveFriendRequests : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				var handShakeKeeper = await HandShakeKeeper.CreateAsync(
					GlobalSettings.MyAccount,
					GlobalSettings.MyPassword
				);
				if (handShakeKeeper != null)
				{
					var getFriends = new EvoApiRequester<
					evoapi.ProtocolModels.Social.GetFriends.Request,
					evoapi.ProtocolModels.Social.GetFriends.Response>();
					var resGetFriends = await getFriends.PostAsync(
						$"{handShakeKeeper.Token}"
					);

					var responseFriendRequest = new EvoApiRequester<
						evoapi.ProtocolModels.Social.ResponseFriendRequest.Request,
						evoapi.ProtocolModels.Social.ResponseFriendRequest.Response>();
					responseFriendRequest.request.approved = true;
					foreach (var req in resGetFriends.Payload.lists.Requests)
					{
						responseFriendRequest.request.playerId = req.playerId;

						var resResponseFriendRequest = await responseFriendRequest.PostAsync(
							$"{handShakeKeeper.Token}"
						);

						if (resResponseFriendRequest.StatusCode != System.Net.HttpStatusCode.OK
						|| null == resResponseFriendRequest.Payload.lists)
						{
							ConsoleWriter.Warning($"Response is Failed (playerId:{req.playerId}).");
							continue;
						}

						ConsoleWriter.Succeeded($"Added to friends (playerId:{req.playerId}).");
					}

					await handShakeKeeper.CancelAsync();
				}

				StateMachine.SendEvent(Main.EventTrigger.Next);
			});
		}
	}
}
