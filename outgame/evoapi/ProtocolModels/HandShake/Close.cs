
namespace evoapi.ProtocolModels.HandShake
{
	public class Close : HandShake
	{
		public class Response : ResponseBase
		{
			public string reason { get; set; }
		}
	}
}
