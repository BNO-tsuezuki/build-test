
namespace evoapi.ProtocolModels.HandShake
{
	public class RecievedFriendRequest : HandShake
	{
		public class Response : ResponseBase
		{
			public long playerId { get; set; }
		}
	}
}
