using System;
using System.Collections.Generic;

using evolib.Log;


namespace evomatching.Matching
{
	public class BattleEntryManager
	{
		class BattleEntryPlayer : IBattleEntryPlayer
		{
			public long PlayerId { get; set; }
			public string PlayerName { get; set; }
			public float Rating { get; set; }
			public string SessionId { get; set; }
		}

		class BattleEntry : IBattleEntry
		{
			public ulong EntryId { get; set; }

			public List<IBattleEntryPlayer> _Players { get; set; }
			public IReadOnlyList<IBattleEntryPlayer> Players
			{
				get { return _Players; }
			}

			public DateTime EntryTime { get; set; }
			public TimeSpan WaitingTime { get { return DateTime.UtcNow - EntryTime; } }


			public evolib.Battle.MatchType MatchType { get; set; }


			public float RatingSum { get; private set; }
			public float RatingAvg { get; private set; }
			public void RecalcRating()
			{
				if (Players.Count == 0)
				{
					RatingSum = 0;
					RatingAvg = 0;
					return;
				}

				float ratingSum = 0;
				for( int i=0; i<Players.Count; i++ )
				{
					var p = Players[i];
					ratingSum += p.Rating;
				}

				var coefficient = 1f;
				//coefficient += ((Players.Count - 1) * 0.05f);// TODO ★グループによる割り増し評価

				RatingSum = ratingSum * coefficient;
				RatingAvg = RatingSum / Players.Count;
			}

			public UInt64 MinPackageVersion { get; set; }
		}

		ulong SerialEntryId = 12345;

		SortedDictionary<ulong, BattleEntry> Entries = new SortedDictionary<ulong, BattleEntry>();

		Dictionary<long, BattleEntry> PlayerIdMap = new Dictionary<long, BattleEntry>();

		public int EntriedPlayersCount
		{
			get { return PlayerIdMap.Count; }
		}

		public IBattleEntry GetEntry(ulong entryId)
		{
			if( Entries.ContainsKey(entryId))
			{
				return Entries[entryId];
			}

			return null;
		}

		public List<IBattleEntry> GetEntries(Func<IBattleEntry,bool> filter, int num = int.MaxValue)
		{
			var ret = new List<IBattleEntry>();
			foreach( var entry in Entries)
			{
				if (!filter(entry.Value)) continue;

				ret.Add(entry.Value);
				if (num <= ret.Count)
				{
					break;
				}
			}
			return ret;
		}

		public static IBattleEntryPlayer IBattleEntryPlayer(long playerId, string playerName, float rating, string sessionId)
		{
			return new BattleEntryPlayer
			{
				PlayerId = playerId,
				PlayerName = playerName,
				Rating = rating,
				SessionId = sessionId,
			};
		}

		public IReadOnlyList<IBattleEntryPlayer> Entry( 
			evolib.Battle.MatchType matchType,
			UInt64 minPackageVersion,
			List<IBattleEntryPlayer> players)
		{
			foreach (var p in players)
			{
				if (PlayerIdMap.ContainsKey(p.PlayerId))
				{
					return new List<IBattleEntryPlayer>();
				}
			}

			var entry = new BattleEntry()
			{
				EntryId = SerialEntryId++,
				_Players = players,
				EntryTime = DateTime.UtcNow,
				MatchType = matchType,
				MinPackageVersion = minPackageVersion,
			};
			entry.RecalcRating();

			entry._Players.ForEach(p => PlayerIdMap[p.PlayerId] = entry);
			Entries[entry.EntryId] = entry;

			return entry.Players;
		}

		public IReadOnlyList<IBattleEntryPlayer> CancelByPlayer(long playerId)
		{
			if( !PlayerIdMap.ContainsKey(playerId))
			{
				return new List<IBattleEntryPlayer>();
			}

			var entry = PlayerIdMap[playerId];
            return Cancel(entry.EntryId, "", evolib.BattleEntry.Type.Cancel);
		}

		public IReadOnlyList<IBattleEntryPlayer> Cancel(ulong entryId, string matchId, evolib.BattleEntry.Type type)
		{
			if (!Entries.ContainsKey(entryId))
			{
				return new List<IBattleEntryPlayer>();
			}

			var entry = Entries[entryId];

			entry._Players.ForEach(p =>
			{
				PlayerIdMap.Remove(p.PlayerId);

                Logger.Logging(
                    new LogObj().AddChild(new LogModels.LeavePlayer
                    {
                        PlayerId = p.PlayerId,
                        EntryDate = entry.EntryTime,
                        Date = DateTime.UtcNow,
                        Type = type,
                        Rating = p.Rating,
                        MatchId = matchId,
                    })
                );
            });

			Entries.Remove(entry.EntryId);

			return entry.Players;
		}

		public bool Entried(long playerId)
		{
			return PlayerIdMap.ContainsKey(playerId);
		}

		public bool Leave(long playerId, evolib.BattleEntry.Type type)
		{
			if (!PlayerIdMap.ContainsKey(playerId))
			{
				return false;
			}

			var entry = PlayerIdMap[playerId];
			PlayerIdMap.Remove(playerId);

            var leavePlayer = entry._Players.Find(p => p.PlayerId == playerId);
            if (leavePlayer != null)
            {
                Logger.Logging(
                    new LogObj().AddChild(new LogModels.LeavePlayer
                    {
                        PlayerId = playerId,
                        EntryDate = entry.EntryTime,
                        Date = DateTime.UtcNow,
                        Type = type,
                        Rating = leavePlayer.Rating,
                        MatchId = "",
                    })
                );
            }

            var idx = entry._Players.FindIndex(p => p.PlayerId == playerId);
			entry._Players.RemoveAt(idx);

			if (entry.Players.Count == 0)
			{
				Entries.Remove(entry.EntryId);
			}
			else
			{
				entry.RecalcRating();
			}

			return true;
		}



		public void CheckSessions(Func<string, bool> aliveChecker)
		{
			var delPlayerIdList = new List<long>();

			foreach (var entry in Entries.Values)
			{
				entry._Players.ForEach(p =>
				{
					if (!aliveChecker(p.SessionId))
					{
						delPlayerIdList.Add(p.PlayerId);
					}
				});
			}

			delPlayerIdList.ForEach(pid =>
			{
				Leave(pid, evolib.BattleEntry.Type.SessionError);
			});
		}
	}
}
