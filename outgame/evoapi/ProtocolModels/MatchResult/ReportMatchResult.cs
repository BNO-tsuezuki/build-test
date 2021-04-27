
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.MatchResult
{
	public class ReportMatchResult
	{
        public class BattlePassGainExpDetailCommon
        {
            public int addTotalExp { get; set; }
            public int matchWin { get; set; }
            public int matchLose { get; set; }
            public int matchDraw { get; set; }
            public int chosenByMVP { get; set; }
            public int award { get; set; }
            public int kill { get; set; }
            public int recover { get; set; }
        }

        public class BattlePassGainExpDetail : BattlePassGainExpDetailCommon
        {
            [Range((int)evolib.BattlePass.PassType.SeasonPass, (int)evolib.BattlePass.PassType.PlayerLevel)]
            public evolib.BattlePass.PassType passType { get; set; }
            public int passId { get; set; }
        }

        public class ChallengeInfo
        {
            public string challengeId { get; set; }
            public int count { get; set; }
        }

        // DedicatedServerから送られてくる、プレイヤーのリザルト情報
        public class MatchResultDetail
		{
			[Required]
			public long playerId { get; set; }

			public string playerName { get; set; }
			public evolib.Battle.Side side { get; set; }
			public evolib.Battle.Result result { get; set; }
			// battle pass
			public List<BattlePassGainExpDetail> battlePassGainExpDetails { get; set; }

			// challenge
			public List<ChallengeInfo> challenges { get; set; }
		}

        public class Request
		{
            public List<MatchResultDetail> results { get; set; }
        }

        public class BattlePassProgressInMatchResult
        {
            public int level { get; set; }
            public UInt64 totalExp { get; set; }
            // 現在のレベルにおける経験値
            public UInt64 expInlevel { get; set; }
            // 次のレベルアップに必要な経験値
            public UInt64 nextExp { get; set; }
            public List<string> rewardIds { get; set; }
        }

        public class PlayersBattlePassMatchResult
        {
            public int passId { get; set; }
            public evolib.BattlePass.PassType passType { get; set; }

            // リザルト画面での比較に使用するプレイヤーレベルとバトルパスの進捗(経験値適用前)
            public BattlePassProgressInMatchResult beforeProgress { get; set; }
            // リザルト画面での比較に使用するプレイヤーレベルとバトルパスの進捗(経験値適用後)
            public BattlePassProgressInMatchResult afterProgress { get; set; }
            // 獲得経験値詳細
            public BattlePassGainExpDetailCommon gainExpDetail;
        }

        // プレイヤーのリザルト適用情報
        public class AppliedPlayersMatchResult
        {
			public long playerId { get; set; }
            public List<PlayersBattlePassMatchResult> battlePassResults;
        }

        public class Response
        {
            public List<AppliedPlayersMatchResult> appliedPlayersMatchResults;
        }
    }
}
