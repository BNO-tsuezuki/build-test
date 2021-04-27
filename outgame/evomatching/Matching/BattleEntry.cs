using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public interface IBattleEntryPlayer
	{
		long PlayerId { get; }
		string PlayerName { get; }
		float Rating { get; }
		string SessionId { get; }
	}

	public interface IBattleEntry
	{
		ulong EntryId { get; }
		IReadOnlyList<IBattleEntryPlayer> Players { get; }
		TimeSpan WaitingTime { get; }
		evolib.Battle.MatchType MatchType { get; }

		float RatingSum { get; }
		float RatingAvg { get; }

		UInt64 MinPackageVersion { get; }
	}
}
