using System.Collections.Generic;

public class HandShake : Protocol<HandShake, HandShake.Req, HandShake.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/HandShake"; }
	}

	[System.Serializable]
	public class Req
	{
	}

	[System.Serializable]
	public class Res
	{
		public string pushCode;
		public int nextResponseSeconds;
		public int[] masterDataVersion;
		public bool enabledMatchmake;
		public string[] opsNoticeCodes;
		public string[] disabledMobileSuits;
	}

	public class Close:Res
	{
		public string reason;
	}

	// ----------------
	// For Player
	// ----------------
	public class JoinBattle:Res
	{
		public string ipAddr;
		public int port;
		public string joinPassword;
		public string token;
		public string newEncryptionKey;
	}

	public class Chat : Res
	{
		public int type;
		public long playerId;
		public string playerName;
		public string text;
	}

	// ----------------
	// For BattleServer
	// ----------------
	public class ChangeBattlePhase : Res
	{
		public string phase;
	}
	public class ExecCommand : Res
	{
		public string command;
	}
	public class MatchInfo : Res
	{
		public string matchId;
		public int matchType;
	}
}
