using System;
using System.Collections.Generic;
using System.Text;

namespace evolib.Log
{
	// 同一Tag名でことなる中身が存在することを防止すため
	// すべてのLogModelはここで定義されるべき
	public class LogModels
	{
		public class InfoReport
		{
			public string Msg { get; set; }
		}

		public class ErrorReport
		{
			public string Msg { get; set; }
			public string Exception { get; set; }
		}

		public class ServerStart
		{
			public DateTime Date { get; set; }
		}

		public class MasterDataStartLoading
		{
			public string Path { get; set; }
			public DateTime UpdateDate { get; set; }
			public DateTime Date { get; set; }
		}
		public class MasterDataEndLoading : MasterDataStartLoading
		{
		}

		public class DataBase
		{
			public string State { get; set; }
		}

		public class HttpRequest
		{
			public string Url { get; set; }
			public string TraceId { get; set; }
			public string RemoteIp { get; set; }
			public string Response { get; set; }
			public int StatusCode { get; set; }
			public long ElapsedTime { get; set; }
			public DateTime RequestDate { get; set; }
			public DateTime ResponseDate { get; set; }


			public class Host
			{
				public string SessionId { get; set; }
				public string Authorization { get; set; }
				public DateTime LoginDate { get; set; }
				public string Account { get; set; }
				public Account.Type AccountType { get; set; }
				public HostType HostType { get; set; }
				public long PlayerId { get; set; }
				public string PlayerName { get; set; }


				public class HandShake
				{
					public bool Aborted { get; set; }
					public DateTime UpdateTTL { get; set; }
				}
			}
		}



		//----------------------------------------------------------------------------
		//----------------------------------------------------------------------------
		//----------------------------------------------------------------------------
		//----------------------------------------------------------------------------

		public class CreatePlayer
		{
			public long PlayerId { get; set; }
			public DateTime Date { get; set; }
			public string PlayerName { get; set; } //platformによっては既存
			public Account.Type AccountType { get; set; }
            public string CountryCode { get; set; }
        }

		public class Login
		{
			public long PlayerId { get; set; }
			public DateTime Date { get; set; }
			public string RemoteIp { get; set; }
            public string PlatformInfo { get; set; }
            public string OsInfo { get; set; }
            public string HddUuid { get; set; }
            public string MacAddress { get; set; }

            //public long 課金EC所持数 { get; set; }
            //public long 無課金EC所持数 { get; set; }
            //public long CP所持数 { get; set; }
            //public long MP所持数 { get; set; }
        }

		public class Logout
		{
			public long PlayerId { get; set; }
			public DateTime Date { get; set; }
			public string RemoteIp { get; set; }
		}

        public class SetOpenType
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public evolib.PlayerInformation.OpenType OpenType { get; set; }
        }

        public class ChangePretendOffline
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public bool PretendOffline { get; set; }
        }

        public class SayChat
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
			public evolib.Chat.Type ChatType { get; set; }
            public string GroupId { get; set; }
            public string MatchId { get; set; }
            public evolib.Battle.Side Side { get; set; }
            public string Text { get; set; }
        }

        public class WhisperChat
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public string Text { get; set; }
			public long TargetPlayerId { get; set; }
        }

        public class SetAppOption
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public int? Category { get; set; }
            public int[] Values { get; set; }
        }

        public class ChangePlayerExp
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public int Exp { get; set; }
            public UInt64 TotalExp { get; set; }
            public int Level { get; set; }
        }

        public class ChangeCustomItem
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public string UnitId { get; set; }
            public evolib.Item.Type ItemType { get; set; }
            public string ItemId { get; set; }
        }

        public class ChangePalletItem
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public string UnitId { get; set; }
            public evolib.Item.Type ItemType { get; set; }
            public List<string> ItemIds { get; set; }
        }

        public class ChangePackItem
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public string UnitId { get; set; }
            public evolib.Item.Type ItemType { get; set; }
            public string ItemId { get; set; }
            public List<string> ItemIds { get; set; }
        }

        public class TutorialProgress
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public evolib.PlayerInformation.TutorialType TutorialType { get; set; }
        }

        public class SendInvitationParty
        {
            public long PlayerIdSrc { get; set; }
            public long PlayerIdDst { get; set; }
            public DateTime Date { get; set; }
        }

        public class ResponseInvitationParty
        {
            public long PlayerIdSrc { get; set; }
            public long PlayerIdDst { get; set; }
            public DateTime Date { get; set; }
            public evolib.PremadeGroup.ResponseType Type { get; set; }
        }

        public class UpdateParty
        {
            public string GroupId { get; set; }
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public evolib.PremadeGroup.Type Type { get; set; }
            public List<long> PlayerIds { get; set; }
        }

        public class SendFriend
        {
            public long PlayerIdSrc { get; set; }
            public long PlayerIdDst { get; set; }
            public DateTime Date { get; set; }
        }

        public class ResponseFriend
        {
            public long PlayerIdSrc { get; set; }
            public long PlayerIdDst { get; set; }
            public DateTime Date { get; set; }
            public evolib.Social.ResponseType Type { get; set; }
        }

        public class RuptureFriend
        {
            public long PlayerIdSrc { get; set; }
            public long PlayerIdDst { get; set; }
            public DateTime Date { get; set; }
        }

        public class EntryPlayer
        {
            public DateTime Date { get; set; }
            public evolib.Battle.MatchType MatchType { get; set; }
            public string GroupId { get; set; }
            public List<long> PlayerIds { get; set; }
        }

        public class LeavePlayer
        {
            public long PlayerId { get; set; }
            public DateTime EntryDate { get; set; }
            public DateTime Date { get; set; }
            public evolib.BattleEntry.Type Type { get; set; }
            public float Rating { get; set; }
            public string MatchId { get; set; }
        }

		public class CurrentSessionCount
		{
			public int Count { get; set; }
			public string AreaName { get; set; }
			public string BreakDown { get; set; }
			public DateTime Date { get; set; }
			public int enabledMatchesCnt { get; set; }
			public int entriedPlayersCnt { get; set; }
		}

        public class ReportPlayer
        {
			public long ReportingPlayerId { get; set; }
			public DateTime Date { get; set; }
			public long PlayerId { get; set; }
            public string MatchId { get; set; }
			public bool Uncooperative { get; set; }
			public bool Sabotage { get; set; }
			public bool Cheat { get; set; }
			public bool Harassment { get; set; }
			public bool Abuse { get; set; }
			public bool HateSpeech { get; set; }
            public bool InappropriateName { get; set; }
    		public string Comment { get; set; }
        }

		public class PingResults
		{
			public long PlayerId { get; set; }
			public class Result
			{
				public string regionCode { get; set; }
				public int time { get; set; }
			}
			public List<Result> Results { get; set; }
		}

        public class PlaySupplyPod
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public string SupplypodId { get; set; }
            public SupplyPod.ConsumeType Type { get; set; }
            public int ExecutionNum { get; set; }
            public int PodNum { get; set; }
            public Item.Type ItemType { get; set; }
            public string ItemId { get; set; }
            public GiveAndTake.GiveResult GivenCode { get; set; }
            public long Amount { get; set; }
        }

        public class GivePresent
        {
            public long PlayerId { get; set; }
            public DateTime Date { get; set; }
            public GiveAndTake.Type Type { get; set; }
            public string Id { get; set; }
            public long Amount { get; set; }
            public DateTime ExpirationDate { get; set; }
            public PresentBox.Type RouteType { get; set; }
            public PresentBox.Code Code { get; set; }
            public GiveAndTake.GiveResult GivenCode { get; set; }
        }

		public class Discipline
		{
			public long playerId { get; set; }

			public evolib.Discipline.Level level { get; set; }

			public string title { get; set; }

			public string text { get; set; }

			public DateTime expirationDate { get; set; }
		}

	}
}
