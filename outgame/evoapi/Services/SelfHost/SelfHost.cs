
namespace evoapi.Services.SelfHost
{
	public interface ISelfHost
	{
		string sessionId { get; }

		string signingKey { get; }
		System.DateTime loginDate { get; }
		string account { get; }
		evolib.Account.Type accountType { get; }
		string accountAccessToken { get; }
		evolib.MatchingArea matchingArea { get; }


		evolib.HostType hostType { get; }
		IPlayerInfo playerInfo { get; }
		IBattleServerInfo battleServerInfo { get; }
	}

	public class SelfHost : ISelfHost
	{
		public string sessionId { get; set; }

		public string signingKey { get; set; }
		public System.DateTime loginDate { get; set; }
		public string account { get; set; }
		public evolib.Account.Type accountType { get; set; }
		public string accountAccessToken { get; set; }
		public evolib.MatchingArea matchingArea { get; set; }


		public evolib.HostType hostType { get; set; }
		public IPlayerInfo playerInfo { get; set; }
		public IBattleServerInfo battleServerInfo { get; set; }
	}
}
