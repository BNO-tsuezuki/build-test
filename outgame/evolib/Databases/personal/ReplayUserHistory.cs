//
// 1回のマッチに対応するリプレイ検索用情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
    // 各ユーザの参加試合の検索用データ
    public class ReplayUserHistory
	{
        // key 1/2
        [Required]
        public long playerId { get; set; }

        // key 2/2
        [Required]
        public DateTime date { get; set; }

        [MaxLength(64)]
        public string matchId { get; set; }

        public evolib.Battle.MatchType matchType { get; set; }//勝利、敗北など

        public evolib.Battle.Result resultType { get; set; }

		public UInt64 packageVersion { get; set; }

		public UInt64 masterDataVersion { get; set; }
	}
}
