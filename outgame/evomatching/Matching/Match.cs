using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public enum MatchState
	{
		Matching,
		Ready,
		Starting,
		Result,
		Closing,
	}

	public enum MatchPlayerState
	{
		Assigned,
		Reassigned,
		Joined,
	}

	public interface IMatchPlayer
	{
		long PlayerId { get; }
		string PlayerName { get; }
		string SessionId { get; }
		float Rating { get; }
		int GroupNo { get; }
		evolib.Battle.Side Side { get; }
		DateTime MatchingTime { get; }
		string JoinPassword { get; }

		MatchPlayerState State { get; }
	}

	public interface IBattleServer
	{
		string SessionId { get; }
		string IpAddr { get; }
		int Port { get; }
		string Rule { get; }
		string MapId { get; }
		string Label { get; }
		string Description { get; }
		string ServerName { get; }
		string Region { get; }
		string Owner { get; }
		bool AutoMatchmakeTarget { get; }
		DateTime EntryTime { get; }
	}

	public interface IMatch
	{
		string MatchId { get;  }

		MatchState State { get;  }

		IReadOnlyList<IMatchPlayer> Players { get; }

		IBattleServer Server { get;  }

		DateTime ReadyTime { get; }
	}

	public static class IMatchExtensions
	{
		public static evolib.Kvs.Models.ConnectionQueueData.MatchInfo MatchInfo(this IMatch match)
		{
			var ret = new evolib.Kvs.Models.ConnectionQueueData.MatchInfo
			{
				matchId = match.MatchId,
				matchType = evolib.Battle.MatchType.Casual,// TODO match.MatchType,
				players = new List<evolib.Kvs.Models.ConnectionQueueData.MatchInfo.Player>(),
			};
			for (int i = 0; i < match.Players.Count; i++)
			{
				var p = match.Players[i];
				ret.players.Add(new evolib.Kvs.Models.ConnectionQueueData.MatchInfo.Player
				{
					playerId = p.PlayerId,
					groupNo = p.GroupNo,
					side = p.Side,
				});
			}

			return ret;
		}

		public static evolib.Kvs.Models.ConnectionQueueData.JoinBattleServer JoinBattleServer(this IMatch match, long playerId )
		{
			var players = new List<evolib.Kvs.Models.ConnectionQueueData.JoinBattleServer.Player>();

			for (int i = 0; i < match.Players.Count; i++)
			{
				var p = match.Players[i];
				players.Add(new evolib.Kvs.Models.ConnectionQueueData.JoinBattleServer.Player
				{
					playerId = p.PlayerId,
					groupNo = p.GroupNo,
					side = p.Side,
				});
			}

			for (int i = 0; i < match.Players.Count; i++)
			{
				var p = match.Players[i];
				if (p.PlayerId != playerId) continue;

				return new evolib.Kvs.Models.ConnectionQueueData.JoinBattleServer
				{
					ipAddr = match.Server.IpAddr,
					port = match.Server.Port,
					joinPassword = p.JoinPassword,
					mapId = match.Server.MapId,
					rule = match.Server.Rule,
					matchId = match.MatchId,
					matchType = evolib.Battle.MatchType.Casual,// TODO match.MatchType,
					players = players
				};
			}

			return null;
		}
	}
}
