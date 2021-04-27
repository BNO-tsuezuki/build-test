using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evomatching.Matching
{
	public interface IPremadeGroupPlayer
	{
		long PlayerId { get; }
		string SessionId { get; }
	}

	public interface IPremadeGroup
	{
		string GroupId { get; }

		IReadOnlyList<IPremadeGroupPlayer> Players { get; }

		long LeaderPlayerId { get; }
	}
}
