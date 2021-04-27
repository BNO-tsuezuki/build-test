using System;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class ClearChallenge : HandShake
	{
		public class Response : ResponseBase
		{
			public List<string> challengeIds { get; set; }
		}
	}
}
