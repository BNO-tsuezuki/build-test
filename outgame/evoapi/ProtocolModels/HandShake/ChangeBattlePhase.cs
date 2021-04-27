

namespace evoapi.ProtocolModels.HandShake
{
    public class ChangeBattlePhase : HandShake
	{
		public class Response : ResponseBase
		{
			public string phase { get; set; }
		}
	}
}
