
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.ViewMatch
{
	public class SearchAllReplay
	{
        public class Request
		{
			[Required]
			public int startIndex { get; set; }

            [Required]
            [Range(1, 100)]
            public int? getNum { get; set; }
        }

        public class UserReplayInfo
        {
            public DateTime date { get; set; }
            public evolib.Battle.MatchType matchType { get; set; }
            public int gameMode { get; set; }
            public int totalTime { get; set; }
            public int mvpUnit { get; set; }
            public int ratingAverage { get; set; }
            public int viewNum { get; set; }
            public string matchId { get; set; }
            public string result { get; set; }
            public string mapId { get; set; }
            public string mvpPlayer { get; set; }
        }

        public class Response
		{
            public List<UserReplayInfo> ReplayInfos { get; set; }
		}
	}
}
