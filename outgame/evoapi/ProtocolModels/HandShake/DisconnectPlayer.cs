using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class DisconnectPlayer : HandShake
	{
		public class Response : ResponseBase
		{
			public List<long> players { get; set; }
		}
	}
}
