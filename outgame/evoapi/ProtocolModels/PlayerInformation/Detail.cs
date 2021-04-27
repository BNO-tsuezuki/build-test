using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.PlayerInformation
{
	public class Detail
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }
		}

		public class Response
		{
			public Basic.Info basicInfo { get; set; }

			public UInt64 exp { get; set; }
			public UInt64 nextLevelExp { get; set; }

			public int battleRating { get; set; }
			public int battleRatingMax { get; set; }
			public int battleRatingPrevMax { get; set; }

			public evolib.PlayerInformation.OpenType openType { get; set; }

			public List<UseMobileSuitInfo> useMobileSuitInfos { get; set; }
		}
	}
}
