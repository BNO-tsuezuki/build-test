
namespace evoapi.Services.SelfHost
{
	public interface IPlayerInfo
	{
		long playerId { get; }
		string playerName { get; }
		float battleRating { get; }
		string playerIconItemId { get; }
		bool pretendedOffline { get; }
	}

	public class PlayerInfo : IPlayerInfo
	{
		public long playerId { get; set; }
		public string playerName { get; set; }
		public float battleRating { get; set; }
		public string playerIconItemId { get; set; }
		public bool pretendedOffline { get; set; }
	}
}
