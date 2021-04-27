using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using evolib;
using evoapi.ProtocolModels.PlayerInformation;

namespace TestSituationSetter.State.MainStates
{
	public class RecentPlayers : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				var login = new EvoApiRequester<
						   evoapi.ProtocolModels.Auth.Login.Request,
						   evoapi.ProtocolModels.Auth.Login.Response>();

				var personals =
					new Queue<ReportBattleResult.Personal>();

				{
					var matchCnt = (evolib.Social.MaxRecentPlayersCnt
										+ evolib.Battle.MatchPlayersNum - 1 - 1)
												/ (evolib.Battle.MatchPlayersNum - 1);

					var stackNum = matchCnt * (evolib.Battle.MatchPlayersNum - 1);

					foreach (var account in TestAccounts.Actors())
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

						if (0 == (resLogin.Payload.initialLevel
									& evolib.PlayerInformation.InitialLevelFlg.Name))
						{
							ConsoleWriter.Error("Player has no name.");
							break;
						}

						personals.Enqueue(new ReportBattleResult.Personal
							{
								playerId = resLogin.Payload.playerId,
								playerName = account.Name,
								result = evolib.Battle.Result.DRAW,
								side = (personals.Count % 2 == 0)
									? evolib.Battle.Side.Earthnoid
									: evolib.Battle.Side.Spacenoid,
							}
						);

						ConsoleWriter.Action($"{account.Account} Logined...");

						if (stackNum == personals.Count)
						{
							break;
						}
					}
				}

				var handShakeKeeper = await HandShakeKeeper.CreateAsync(
					TestAccounts.DsAccount,
					TestAccounts.DsPassword
				);

				if (handShakeKeeper != null)
				{
					var reportBattleResult = new EvoApiRequester<
						ReportBattleResult.Request,
						ReportBattleResult.Response>();


					while (true)
					{
						var personalList = new List<ReportBattleResult.Personal>();
						for (int i = 0; i < Battle.MatchPlayersNum - 1; i++)
						{
							if (0 < personals.Count)
							{
								personalList.Add(personals.Dequeue());
							}
						}
						if (personalList.Count == 0) break;

						personalList.Add(new ReportBattleResult.Personal
						{
							playerId = GlobalSettings.MyPlayerId,
							playerName = "abcd",
							result = Battle.Result.DRAW,
							side = personalList[personalList.Count - 1].side.Opponet(),
						});

						reportBattleResult.request.personals = personalList.ToArray();

						var resReportBattleResult = await reportBattleResult.PostAsync(
								$"{handShakeKeeper.Token}"
						);

						if (resReportBattleResult.StatusCode != System.Net.HttpStatusCode.OK)
						{
							ConsoleWriter.Error("ReportBattleResult is Failed.");
							break;
						}

						ConsoleWriter.Succeeded("ReportBattleResult is Succeeded.");
					}

					await handShakeKeeper.CancelAsync();
				}

				StateMachine.SendEvent(Main.EventTrigger.Next);
			});
		}
	}
}
