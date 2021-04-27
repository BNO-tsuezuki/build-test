using System;
using System.Collections.Generic;
using System.Text;

namespace MatchmakeTest
{
	public class VirtualMatchManager
	{
		class Match
		{
			public string matchId { get; set; }

			public int finishTime { get; set; }
		}

		List<Match> MatchList = new List<Match>();
		
		public int TotalMatchCount { get; private set; }
		public int MatchCount { get { return MatchList.Count; } }


		public void Matching(int num)
		{
			var rnd = new Random();
			for( int i=0; i<num; i++)
			{
				MatchList.Add(new Match
				{
					matchId = evolib.Util.KeyGen.GetUrlSafe(32),
					finishTime = Program.ElapsedSecond + rnd.Next(20, 25) * 60,
				});
			}

			TotalMatchCount += num;
		}

		public void CheckFinish()
		{
			for (int i = 0; i < MatchList.Count;)
			{
				var match = MatchList[i];

				if (match.finishTime <= Program.ElapsedSecond)
				{
					MatchList.RemoveAt(i);
					continue;
				}

				i++;
			}
		}
	}
}
