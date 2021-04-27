public class MatchingEntryBattleServer : Protocol<MatchingEntryBattleServer, MatchingEntryBattleServer.Req, MatchingEntryBattleServer.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/EntryBattleServer"; }
	}

	[System.Serializable]
	public class Req
	{
		public string ipAddr;
		public int port;
		public string rule;
		public string mapId;
		public string label;
		public string description;
		public bool autoMatchmakeTarget;
		public string serverName;
		public string region;
		public string owner;
	}

	[System.Serializable]
	public class Res
	{
	}
}
