using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using evolib;
using evolib.Kvs.Models;
using evolib.Log;
using evolib.Databases;
using evolib.Databases.common2;

namespace evomatching.Matching
{
	public class GeneralManager
	{
		public PgInvitationManager PgInvitationManager { get; private set; }
		public PremadeGroupManager PremadeGroupManager { get; private set; }
		public BattleEntryManager BattleEntryManager { get; private set; }
		public MatchManager MatchManager { get; private set; }
		public evolib.WatchDogSession WatchDogSession { get; private set; }


		evolib.Util.SequentialJobQueue SequentialJobQueue = new evolib.Util.SequentialJobQueue();
		public Task EnqueueJob(Func<Task> func) { return SequentialJobQueue.Enqueue(func); }
		public Task EnqueueJob(Action func) { return SequentialJobQueue.Enqueue(func); }

		
		public void Start()
		{
			PgInvitationManager = new PgInvitationManager();
			PremadeGroupManager = new PremadeGroupManager();
			BattleEntryManager = new BattleEntryManager();
			MatchManager = new MatchManager();

			var matchMaker = new Logic.Matchmaker2();


			var sessionCheckTimer = new IntervalTimer(5000);
			var autoMatchmakeTimer = new IntervalTimer(30000);
			var generateSessionCntTimer = new IntervalTimer(5000);
			var LogSessionCntTimer = new IntervalTimer(30000);


			Task.Run(async () =>
			{
				while (true)
				{
					if( Settings.MatchingArea != MatchingArea.Unknown)
					{
						WatchDogSession = new WatchDogSession(Settings.MatchingArea);
						break;
					}

					await Task.Delay(1000);
				}

				var enabledMatchesCnt = 0;
				var entriedPlayersCnt = 0;

				while (true)
				{
					// -----------------------------------------
					// Session有効確認
					// -----------------------------------------
					if (sessionCheckTimer.Timeup())
					{
						var tmp = EnqueueJob(async () =>
						{
							BattleEntryManager.CheckSessions(WatchDogSession.Alive);

							MatchManager.CheckSessions(WatchDogSession.Alive);

							PgInvitationManager.ClearExpiredInvitations();//Clear!! ExpiredInvitations

							var changedPlayers = PremadeGroupManager.CheckSessions(WatchDogSession.Alive);

							for ( int i=0; i<changedPlayers.Count; i++ )
							{
								var player = changedPlayers[i];

								await new evolib.Kvs.Models.ConnectionQueue(player.SessionId).EnqueueAsync(
									this.CreatePremadeGroupQueueData(player.PlayerId)
								);
							}

							sessionCheckTimer.Start();
						});
					}

					// -----------------------------------------
					// Auto Matchmake
					// -----------------------------------------
					if (autoMatchmakeTimer.Timeup())
					{
						var tmp = EnqueueJob(async () =>
						{
							var enabledMatches =
								MatchManager.GatherMatches(m =>
								{
									return
										m.State == MatchState.Matching && m.Server.AutoMatchmakeTarget;
								});

							enabledMatchesCnt = enabledMatches.Count;
							entriedPlayersCnt = BattleEntryManager.EntriedPlayersCount;


							if ( VersionChecker.Get(VersionChecker.CheckTarget.EnabledMatchmake).Check())
							{
								var limitPackagerVer = VersionChecker.Valued(
									VersionChecker.ReferenceSrc.PackageVersion,
									VersionChecker.LimitPackageVersion(VersionChecker.CheckTarget.Matchmake)
								);

								var minMatchmakeEntriedPlayersNumber = MinMatchmakeEntriedPlayersNumber.Pop(null);
								using ( var common2Db = DbContextFactory.CreateCommon2())
								{
									minMatchmakeEntriedPlayersNumber = MinMatchmakeEntriedPlayersNumber.Pop(
										await common2Db.GenericDatas.FindAsync(GenericData.Type.MinMatchmakeEntriedPlayersNumber)
									);
								}


								var result = matchMaker.MatchmakeForCBT1(
									enabledMatches.Count,
									BattleEntryManager.GetEntries(e =>
									{
										return limitPackagerVer <= e.MinPackageVersion;
									},
									5000),
									minMatchmakeEntriedPlayersNumber
								);

								// Sort by entryTime.
								enabledMatches.Sort((a, b) => (a.Server.EntryTime < b.Server.EntryTime) ? -1 : 1);

								for (int i = 0; i < result.Count; i++)
								{
									var match = enabledMatches[i];

									var matchmake = result[i];

									var players = new Dictionary<string, long>();

									foreach( var e in matchmake.Elements )
									{
										if (MatchManager.AssignPlayers(
											match.MatchId,
											e.Entry.Players,
											e.Side))
										{
											BattleEntryManager.Cancel(e.Entry.EntryId, match.MatchId, evolib.BattleEntry.Type.Matching);

											foreach( var p in e.Entry.Players)
											{
												players[p.SessionId] = p.PlayerId;
											}
										}
									}

									foreach( var p in players)
									{
										await new ConnectionQueue(p.Key).EnqueueAsync(
											match.JoinBattleServer(p.Value)
										);
									}

									await new ConnectionQueue(match.Server.SessionId).EnqueueAsync(
										match.MatchInfo()
									);
								}
							}

							autoMatchmakeTimer.Start();
						});
					}


					// -----------------------------------------
					// 同時接続数出力
					// -----------------------------------------
					if (generateSessionCntTimer.Timeup())
					{
						var now = DateTime.UtcNow;
						var areaName = Settings.MatchingArea.ToString();

						var total = 0;
						var breakDown = "{";
						foreach (var cnt in WatchDogSession.ClientCounts)
						{
							total += cnt.Value;

							breakDown += $"{cnt.Key}:{cnt.Value},";

						}
						breakDown += "}";

						var currentSessionCount = new CurrentSessionCount(Settings.MatchingArea);
						currentSessionCount.Model.areaName = areaName;
						currentSessionCount.Model.count = total;
						currentSessionCount.Model.date = now;
						currentSessionCount.Model.breakDown = breakDown;
						currentSessionCount.Model.enabledMatchesCnt = enabledMatchesCnt;
						currentSessionCount.Model.entriedPlayersCnt = entriedPlayersCnt;
						await currentSessionCount.SaveAsync(
							TimeSpan.FromMilliseconds(generateSessionCntTimer.Interval*2)
						);

						if (LogSessionCntTimer.Timeup())
						{
							Logger.Logging(
								new LogObj().AddChild(new LogModels.CurrentSessionCount
								{
									AreaName = areaName,
									Count = total,
									BreakDown = breakDown,
									Date = now,
									entriedPlayersCnt = entriedPlayersCnt,
									enabledMatchesCnt = enabledMatchesCnt,
								})
							);

							LogSessionCntTimer.Start();
						}

						generateSessionCntTimer.Start();
					}

					await Task.Delay(100);
				}

			});
		}
	}

	class IntervalTimer
	{
		public double Interval { get; private set; }
		DateTime? Limit { get; set; }

		public IntervalTimer(double interval)
		{
			Interval = interval;
			Start();
		}

		public void Start()
		{
			Limit = DateTime.UtcNow + TimeSpan.FromMilliseconds(Interval);
		}

		public bool Timeup()
		{
			if (Limit.HasValue && Limit < DateTime.UtcNow)
			{
				Limit = null;
				return true;
			}

			return false;
		}
	}
}
