using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

using evolib.Log;

namespace evolib.Kvs.Models
{
	[KvsModel(kvsType = KvsType.Common)]
	public class Session : KvsHashModel<Session.INFC, Session.IMPL>
	{
		public static string PrefixClient { get { return "C"; } }
		public static string PrefixServer { get { return "S"; } }
		public static TimeSpan ExpiredSpan { get; set; }



		public Session(string id) : base(id) { }

		public interface INFC
		{
			// --------
			// Common
			// --------
			string signingKey { get; set; }
			DateTime loginDate { get; set; }
			string account { get; set; }
			Account.Type accountType { get; set; }
			string accountAccessToken { get; set; }
			HostType hostType { get; set; }
			int initialLevel { get; set; }
			MatchingArea matchingArea { get; set; }


			// --------
			// Player
			// --------
			long playerId { get; set; }
			DateTime lastOnlineStamp { get; set; }
			bool pretendedOffline { get; set; }
			bool banned { get; set; }

			// --------
			// BattleServer
			// --------
			string serverId { get; set; }
			string matchId { get; set; }

		}

		public class IMPL : INFC
		{
			// --------
			// Common
			// --------
			public string signingKey { get; set; }
			public DateTime loginDate { get; set; }
			public string account { get; set; }
			public Account.Type accountType { get; set; }
			public string accountAccessToken { get; set; }
			public HostType hostType { get; set; }
			public int initialLevel { get; set; }
			public MatchingArea matchingArea { get; set; }


			// --------
			// Player
			// --------
			public long playerId { get; set; }
			public DateTime lastOnlineStamp { get; set; }
			public bool pretendedOffline { get; set; }
			public bool banned { get; set; }

			// --------
			// BattleServer
			// --------
			public string serverId { get; set; }
			public string matchId { get; set; }

		}

		public new async Task<DateTime> SaveAsync(TimeSpan? timeSpan = null)
		{
			var span = timeSpan.HasValue
				? timeSpan
				: ExpiredSpan;

			await base.SaveAsync(span);

			return DateTime.UtcNow + span.Value;
		}
	}
}
