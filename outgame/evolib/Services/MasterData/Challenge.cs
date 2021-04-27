using System;
using System.Collections.Generic;

namespace evolib.Services.MasterData
{
	public interface IChallengeExpiration
	{
		evolib.Challenge.Type type { get; }

		TimeSpan expirationOffsetTime { get; }
	}

	public class ChallengeExpiration : IChallengeExpiration
	{
		public evolib.Challenge.Type type { get; set; }

		public TimeSpan expirationOffsetTime { get; set; }
	}

	public interface IChallenge
	{
		string challengeId { get; }

		string sheetId { get; }

		evolib.Challenge.Type type { get; }

		evolib.Challenge.ReporterType reporterType { get; }

		evolib.Challenge.OutgameServerChallengeType outgameServerChallengeType { get; }

		int requiredNum { get; }
	}

	public class Challenge : IChallenge
	{
		public string challengeId { get; set; }

		public string sheetId { get; set; }

		public evolib.Challenge.Type type { get; set; }

		public evolib.Challenge.ReporterType reporterType { get; set; }

		public evolib.Challenge.OutgameServerChallengeType outgameServerChallengeType { get; set; }

		public int requiredNum { get; set; }
	}

	public interface IBeginnerChallengeSheet
	{
		string sheetId { get; }

		int order { get; }
	}

	public class BeginnerChallengeSheet : IBeginnerChallengeSheet
	{
		public string sheetId { get; set; }

		public int order { get; set; }
	}
}
