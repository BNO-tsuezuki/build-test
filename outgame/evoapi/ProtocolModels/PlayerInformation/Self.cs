using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class UseMobileSuitInfo
	{
		public string mobileSuitId { get; set; }
		public float useRate { get; set; }
	}

	public class Self
	{
		public class Request
		{
		}

		public class Response
		{
			public long playerId { get; set; }
			public string playerName { get; set; }
			public string playerIconItemId { get; set; }

			public int playerLevel { get; set; }
			public UInt64 exp { get; set; }
			public UInt64 nextLevelExp { get; set; }

			public int battleRating { get; set; }
			public int battleRatingMax { get; set; }
			public int battleRatingPrevMax { get; set; }

			public evolib.PlayerInformation.OpenType openType { get; set; }
			public bool pretendOffline { get; set; }

			public List<UseMobileSuitInfo> useMobileSuitInfos { get; set; }
		}
	}
}
