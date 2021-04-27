//
// 1回のマッチに対応するリプレイ検索用情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common3
{
    // 全マッチの基本情報
    public class ReplayInfoAllMatch
	{
        // key 1/2
        [Required]
        public DateTime date { get; set; }

        // key 2/2
        [Required]
        [MaxLength(64)]
        public string matchId { get; set; }

        public evolib.Battle.MatchType matchType { get; set; }

        [MaxLength(16)]
        public string mapId { get; set; }

        public int gameMode { get; set; }

        public int totalTime { get; set; }

        [MaxLength(16)]
        public string result { get; set; }// 2-3などチームポイントリザルトの表記

		[StringLength(32)]
        public string mvpUserName { get; set; }

        public int mvpUnitId { get; set; }

		public UInt64 packageVersion { get; set; }

		public UInt64 masterDataVersion { get; set; }
	}
}
