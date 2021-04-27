using System.Collections.Generic;

namespace evoapi.ProtocolModels.Achievement
{
	public class PlayerAchievementStatusInfo
	{
		public long playerId { get; set; }

		public List<AchievementStatusInfo> list { get; set; }
	}

	public class SaveStatus
	{
		public class Request
		{
			public List<PlayerAchievementStatusInfo> list { get; set; }
		}

		public class Response
		{
		}
	}
}
