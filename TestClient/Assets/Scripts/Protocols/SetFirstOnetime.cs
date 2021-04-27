public class SetFirstOnetime : Protocol<SetFirstOnetime, SetFirstOnetime.Req, SetFirstOnetime.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/PlayerInformation/SetFirstOnetime"; }
	}

	[System.Serializable]
	public class Req
	{
		public string playerName;
	}

	[System.Serializable]
	public class Res
	{
	}
}
