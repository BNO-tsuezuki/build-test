using System;
using System.Collections.Generic;
using System.Text;

using evomatching.Matching;

namespace MatchmakeTest
{
	public class WaitTimeRanking
	{
		public class Info
		{
			public IBattleEntry Ent { get; set; }
			public bool AlreadyMatchmake { get; set; }
			public TimeSpan WaitingTime { get; set; }
		}

		List<Info> _RankingList = new List<Info>();
		public IReadOnlyList<Info> RankingList { get { return _RankingList; } }


		public void Nominate( IBattleEntry ent, bool alreadyMatchmake)
		{
			var waitingTime = ent.WaitingTime;

			var insertIndex = 0;
			for( ; insertIndex < _RankingList.Count; insertIndex++)
			{
				var info = _RankingList[insertIndex];
				if( info.WaitingTime < waitingTime)
				{
					break;
				}
			}

			_RankingList.Insert(
				insertIndex, 
				new Info
				{
					Ent = ent,
					WaitingTime = waitingTime,
					AlreadyMatchmake = alreadyMatchmake,
				}
			);

			if(Program.WaitTimeRankMax < _RankingList.Count)
			{
				_RankingList.RemoveAt(Program.WaitTimeRankMax);
			}
		}
	}
}
