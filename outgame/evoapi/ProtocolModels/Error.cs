
namespace evoapi.ProtocolModels
{
    public class Error
    {
		public enum LowCode
		{
			____  = 0x0000,

			BadParameter,
			BadRequest,
			ServerInternalError,
			Others,

			DisabledMatchmake,//運営によるマッチメイク停止
			NgPackageVersion,

			NgAuth = 0x0100,
			BannedAccount,
			LoginReject,
			
			NgWord = 0x0200,
			OverlapName, //playerName被り


			SentPgInvitation = 0x0300,
			RecievedPgInvitation,
			CouldNotMatchingEnty,   //何ら化に理由で参戦予約できなかった
			CouldNotMatchingCancel, //何ら化に理由で参戦予約できなかった

			AlreadyFriend = 0x0400,
			AlreadySentFriendRequest,
			AlreadyRecievedFriendRequest,
			FriendRequestsCntLimit,
			HisFriendsCntLimit,
			MyFriendsCntLimit,

			PremadeGroupRecievedInvitationSelf = 0x0500,//自分に招待きてるときに、Ｂさんを招待しちゃった
			PremadeGroupTargetBusy,//招待しようとしたＢさんは、Ｃさんから招待されているかＣさんを招待している
			PremadeGroupAlreadyGroupTarget,//招待しようとしたＢさんはすでに他のパーティに属する
			PremadeGroupAlreadyEntryTarget,//招待しようとしたＢさんはすでにマッチングエントリ中
			PremadeGroupAlreadyBattleTarget,//招待しようとしたＢさんはすでに戦闘中
			PremadeGroupOverLimit,//6にんを超えて招待することはできない
			PremadeGroupResponseTimeup,//返事した招待はすでに時間切れ
			PremadeGroupDifferentArea, //MatchingAreaが異なる


			SupplyPodExpired = 0x0600,//指定のサプライポッドの有効期間切れ
            SupplyPodCouldNotReward,//報酬が獲得できなかった
        }

        public Error()
		{
			error = new Payload();
		}

		public class Payload
		{
			public int errCode { get; set; }
			public int subCode { get; set; }
			public object param1 { get; set; }
		}

		public Payload error { get; private set; }
	}
}
