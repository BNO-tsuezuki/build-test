using System;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Challenge
{
	public class ChallengeStatus
	{
		public string challengeId { get; set; }

		public int num { get; set; }

		public bool completed { get; set; }

		public evolib.Challenge.Type type { get; set; }

		public bool isPaidChallenge { get; set; }

		public bool unlocked { get; set; }

		public DateTime expirationDate { get; set; }
	}

	public class GetList
	{
		public class Request
		{
		}

		public class Response
		{
			public List<ChallengeStatus> list { get; set; }
			public string currentSheet { get; set; }
		}
	}
}
