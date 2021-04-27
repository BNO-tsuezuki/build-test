using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Achievement
{
	public class AchievementStatusInfo
	{
		public string achievementId { get; set; }

		public int value { get; set; }

		public bool notified { get; set; }

		public bool obtained { get; set; }

		public DateTime obtainedDate { get; set; }
	}

	public class GetStatus
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }
		}

		public class Response
		{
			public long playerId { get; set; }
			public List<AchievementStatusInfo> list { get; set; }
		}
	}
}
