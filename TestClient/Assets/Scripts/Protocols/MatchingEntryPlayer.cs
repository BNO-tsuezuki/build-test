public class MatchingEntryPlayer : Protocol<MatchingEntryPlayer, MatchingEntryPlayer.Req, MatchingEntryPlayer.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/EntryPlayer"; }
	}

	[System.Serializable]
	public class Req
	{
		public int matchType;
	}

	[System.Serializable]
	public class Res
	{
	}
}
