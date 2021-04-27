
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.ViewMatch
{
	public class ReplayInfoSave
    {
        public class BaseReplayInfo
        {
            public DateTime date { get; set; }
            public evolib.Battle.MatchType matchType { get; set; }
            public int gameMode { get; set; }
            public int totalTime { get; set; }
            public int mvpUnit { get; set; }
            public string matchId { get; set; }
            public string result { get; set; }
            public string mapId { get; set; }
            public string mvpPlayer { get; set; }
        }

        public class PlayerResult
        {
            public long playerId { get; set; }
            public evolib.Battle.Result result { get; set; }
        }

        public class Request
		{
			[Required]
			public string packageVersion { get; set; }
			[Required]
			public string masterDataVersion { get; set; }
			public int rateAverage { get; set; }
            public bool saveRank { get; set; }
            public bool isFeatured { get; set; }
            public BaseReplayInfo baseInfo { get; set; }
            public string members { get; set; }
            public List<int> spawnUnits { get; set; }
            public List<int> awardUnits { get; set; }
            public List<PlayerResult> playerResult { get; set; }
        }

        public class Response
        {

        }
    }
}
