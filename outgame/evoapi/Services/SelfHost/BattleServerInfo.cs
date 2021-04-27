
namespace evoapi.Services.SelfHost
{
	public interface IBattleServerInfo
	{
		string serverId { get; }
		string matchId { get; }

	}
	public class BattleServerInfo : IBattleServerInfo
	{
		public string serverId { get; set; }
		public string matchId { get; set; }
	}
}
