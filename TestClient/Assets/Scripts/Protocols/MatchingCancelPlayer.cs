public class MatchingCancelPlayer : Protocol<MatchingCancelPlayer, MatchingCancelPlayer.Req, MatchingCancelPlayer.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/CancelPlayer"; }
	}

	[System.Serializable]
	public class Req
	{
	}

	[System.Serializable]
	public class Res
	{
	}
}
