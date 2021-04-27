using System;
using System.Collections.Generic;

namespace evolib
{
	public static class Challenge
	{
		public enum Type
		{
			Daily,
			Weekly,
			Beginner,
		};

		public enum ReporterType
		{
			Client,
			DedicatedServer,
			OutgameServer,
		};

		public enum OutgameServerChallengeType
		{
			None,
			Login,
			PlayerLevel,
		};

		public enum Status
		{
			NotClear = 0,
			Clear,
		}

		public static List<int> DrawLots(int listLength, int num)
		{
			var random = new Random();
			var indexList = new List<int>();

			var count = listLength < num ? listLength : num;
			for (int i = 0; i < count; ++i)
			{
				int index = random.Next(0, listLength);
				indexList.Add(index);
			}

			return indexList;
		}

	}
}
