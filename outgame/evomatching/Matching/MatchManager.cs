using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public class MatchManager
	{
		class MatchPlayer : IMatchPlayer
		{
			public long PlayerId { get; set; }
			public string PlayerName { get; set; }
			public string SessionId { get; set; }
			public float Rating { get; set; }
			public int GroupNo { get; set; }
			public evolib.Battle.Side Side { get; set; }
			public DateTime MatchingTime { get; set; }
			public string JoinPassword { get; set; }

			public MatchPlayerState State { get; set; }
		}

		class BattleServer : IBattleServer
		{
			public string SessionId { get; set; }
			public string IpAddr { get; set; }
			public int Port { get; set; }
			public string Rule { get; set; }
			public string MapId { get; set; }
			public string Label { get; set; }
			public string Description { get; set; }
			public string ServerName { get; set; }
			public string Region { get; set; }
			public string Owner { get; set; }
			public bool AutoMatchmakeTarget { get; set; }
			public DateTime EntryTime { get; set; }
		}

		class Match : IMatch
		{
			public string MatchId { get; set; }
			public MatchState State { get; set; }

			public List<IMatchPlayer> _Players;
			public IReadOnlyList<IMatchPlayer> Players { get { return _Players; } }

			public BattleServer _Server { get; set; }
			public IBattleServer Server { get { return _Server; } }

			public DateTime ReadyTime { get; set; }

			public int CurrentGroupNo { get; set; }
		}


		Dictionary<string, Match> _Matches = new Dictionary<string, Match>();
		Dictionary<string, Match> _AssignedServerMap = new Dictionary<string, Match>();
		Dictionary<long, Match> _AssignedPlayerMap = new Dictionary<long, Match>();

		class AssignedLog
		{
			public string MatchId { get; set; }
			public evolib.Battle.Side Side { get; set; }
		}
		Dictionary<long, AssignedLog> _AssignedLogs = new Dictionary<long, AssignedLog>();


		public List<IMatch> GatherMatches(Predicate<IMatch> test)
		{
			var ret = new List<IMatch>();
			foreach( var match in _Matches)
			{
				if( test(match.Value) )
				{
					ret.Add(match.Value);
				}
			}
			return ret;
		}

		public IMatch GetMatch(string matchId)
		{
			if (!_Matches.ContainsKey(matchId))
			{
				return null;
			}
			return _Matches[matchId];
		}

		public string Entry(string sessionId, string ipAddr, int port,
			string rule, string mapId,
			string serverName, string region, string owner,
			string label, string description,
			bool autoMatchmakeTarget 
		)
		{
			var matchId = evolib.Util.KeyGen.GetUrlSafe(32).ToLower();

			_Matches[matchId] = _AssignedServerMap[sessionId] = new Match
			{
				MatchId = matchId,

				State = MatchState.Matching,

				_Players = new List<IMatchPlayer>(),

				_Server = new BattleServer
				{
					SessionId = sessionId,
					IpAddr = ipAddr,
					Port = port,
					Rule = rule,
					MapId = mapId,
					ServerName = serverName,
					Region = region,
					Owner = owner,
					Label = label,
					Description = description,
					AutoMatchmakeTarget = autoMatchmakeTarget,
					EntryTime = DateTime.UtcNow,
				},
			};

			return matchId;
		}

		public evolib.Battle.Side LastSide( string matchId, long playerId )
		{
			var side = evolib.Battle.Side.Earthnoid;

			if (_AssignedLogs.ContainsKey(playerId) && _AssignedLogs[playerId].MatchId == matchId)
			{
				return _AssignedLogs[playerId].Side;
			}

			return evolib.Battle.Side.Unknown;
		}

		public bool AssignPlayers(
			string matchId, IReadOnlyList<IBattleEntryPlayer> players, evolib.Battle.Side side )
		{
			if ( !_Matches.ContainsKey(matchId))
			{
				return false;
			}
			var match = _Matches[matchId];

			for (int i = 0; i < players.Count; i++)
			{
				var playerId = players[i].PlayerId;
				if (_AssignedPlayerMap.ContainsKey(playerId) && _AssignedPlayerMap[playerId].MatchId != matchId )
				{
					return false;
				}
			}
			
			for (int i = 0; i < players.Count; i++)
			{
				var src = players[i];

				var idx = match._Players.FindIndex(p => p.PlayerId == src.PlayerId);
				if (0 <= idx)
				{
					match._Players.RemoveAt(idx);
				}

				match._Players.Add(new MatchPlayer
				{
					PlayerId = src.PlayerId,
					PlayerName = src.PlayerName,
					SessionId = src.SessionId,
					Rating = src.Rating,
					JoinPassword = evolib.Util.KeyGen.Get(32),
					GroupNo = match.CurrentGroupNo,
					Side = side,
					MatchingTime = DateTime.UtcNow,
					State = MatchPlayerState.Assigned,
				});

				_AssignedPlayerMap[src.PlayerId] = match;

				_AssignedLogs[src.PlayerId] = new AssignedLog
				{
					MatchId = match.MatchId,
					Side = side,
				};
			}

			match.CurrentGroupNo++;

			if (match.State < MatchState.Ready)
			{
				match.State = MatchState.Ready;
			}

			return true;
		}


		public IMatch ReleasePlayer(long playerId)
		{
			if (_AssignedPlayerMap.ContainsKey(playerId))
			{
				var match = _AssignedPlayerMap[playerId];
				var idx = match._Players.FindIndex(p => p.PlayerId == playerId);
				match._Players.RemoveAt(idx);

				_AssignedPlayerMap.Remove(playerId);

				return match;
			}

			return null;
		}

		public IMatch GetAssignedMatch(long playerId)
		{
			if( _AssignedPlayerMap.ContainsKey(playerId))
			{
				return _AssignedPlayerMap[playerId];
			}

			return null;
		}

		public IMatchPlayer AcceptPlayer( string battleServerSessionId, string joinPassword)
		{
			if (!_AssignedServerMap.ContainsKey(battleServerSessionId))
			{
				return null;
			}

			var match = _AssignedServerMap[battleServerSessionId];
			var player = match._Players.Find(p => p.JoinPassword == joinPassword);
			if (player == null)
			{
				return null;
			}

			{//Update
				var p = player as MatchPlayer;
				p.JoinPassword = evolib.Util.KeyGen.Get(32);
				p.State = MatchPlayerState.Joined;
			}

			return player;
		}

		public IMatch DisconnectPlayer(string battleServerSessionId, long playerId)
		{
			if (!_AssignedPlayerMap.ContainsKey(playerId))
			{
				return null;
			}

			var match = _AssignedPlayerMap[playerId];
			if (match.Server.SessionId != battleServerSessionId)
			{
				return null;
			}

			return ReleasePlayer(playerId);
		}

		public void RecieveBattlePhaseReport(string battleServerSessionId, evolib.Battle.Phase phase )
		{
			if( !_AssignedServerMap.ContainsKey(battleServerSessionId))
			{
				return;
			}

			var match = _AssignedServerMap[battleServerSessionId];


			if ( phase == evolib.Battle.Phase.RESULT )
			{
				match.State = MatchState.Result;
			}
			else if ( phase == evolib.Battle.Phase.FINISH )
			{
				match.State = MatchState.Closing;
				match._Players.ForEach(p => _AssignedPlayerMap.Remove(p.PlayerId));
				match._Players.Clear();
			}
		}

		public void CheckSessions(Func<string, bool> aliveChecker)
		{
			var delMatchIdList = new List<string>();

			foreach (var match in _Matches.Values )
			{
				if (!aliveChecker(match.Server.SessionId))
				{
					delMatchIdList.Add(match.MatchId);

					_AssignedServerMap.Remove(match.Server.SessionId);

					match._Players.ForEach(p =>
					{
						_AssignedPlayerMap.Remove(p.PlayerId);
					});
				}
				else
				{
					for( int i=0; i<match._Players.Count; )
					{
						var p = match._Players[i];
						if (!aliveChecker(p.SessionId))
						{
							_AssignedPlayerMap.Remove(p.PlayerId);
							match._Players.RemoveAt(i);
							continue;
						}
						i++;
					}
				}
			}

			delMatchIdList.ForEach(matchId =>
			{
				_Matches.Remove(matchId);
			});
		}

	}
}
