//
// 1回のマッチに対応するリプレイ検索用情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common3
{
    // 見る価値のあるマッチとして選出されたリプレイの視聴数
    // すべてのリプレイデータに存在するわけではない
    // Select,Update共に高頻度で発生するため単純化
	public class ReplayViewNum
	{
        [Key]
        [MaxLength(64)]
        public string matchId { get; set; }

        public int ViewNum { get; set; }
    }
}
