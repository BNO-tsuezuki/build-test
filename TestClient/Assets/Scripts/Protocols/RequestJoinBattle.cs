public class RequestJoinBattle : Protocol<RequestJoinBattle, RequestJoinBattle.Req, RequestJoinBattle.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/RequestJoinBattle"; }
	}

	[System.Serializable]
	public class Req
	{
		public string battleId;
	}

	[System.Serializable]
	public class Res
	{
	}
}
