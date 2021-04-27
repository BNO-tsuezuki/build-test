//
// 1回のマッチに対応するリプレイ検索用情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common3
{
    // 見る価値のあるマッチとして選出されたリプレイの検索データ
    // すべてのリプレイデータに存在するわけではない
    public class ReplayInfoRankMatch
	{
        // key 1/2
        [Required]
        public DateTime date { get; set; }

        // key 2/2
        [Required]
        [MaxLength(64)]
        public string matchId { get; set; }

        public bool isFeatured { get; set; }

        public int ratingAverage { get; set; }

        [MaxLength(16)]
        public string mapId { get; set; }

        public int gameMode { get; set; }

        public int mvpUnitId { get; set; }

		public UInt64 spawnUnits { get; set; }

		public UInt64 awardUnits { get; set; }

		public UInt64 packageVersion { get; set; }

		public UInt64 masterDataVersion { get; set; }
	}
}
