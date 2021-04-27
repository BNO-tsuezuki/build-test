using System;
using System.Collections.Generic;


namespace evolib.Services.MasterData
{
	public interface IAchievement
	{
		string achievementId { get; }

		evolib.Achievement.Type type { get; }

		int value { get; }
	}

    public class Achievement : IAchievement
	{
		public string achievementId { get; set; }

		public evolib.Achievement.Type type { get; set; }

		public int value { get; set; }
	}
}
