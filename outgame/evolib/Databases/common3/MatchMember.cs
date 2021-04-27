//
// 1回のマッチに対応するリプレイ検索用情報
//

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common3
{
    // フィーチャードマッチの参加メンバ情報データ
    // すべてのリプレイデータに存在するわけではない
    public class MatchMember
	{
        [Key]
        [MaxLength(64)]
        public string matchId { get; set; }

        // longtext
        public string playersInfo { get; set; }
    }
}
