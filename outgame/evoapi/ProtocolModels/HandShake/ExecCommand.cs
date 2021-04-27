
namespace evoapi.ProtocolModels.HandShake
{
	public class ExecCommand : HandShake
	{
		public class Response : ResponseBase
		{
			public string command { get; set; }
		}
	}
}
