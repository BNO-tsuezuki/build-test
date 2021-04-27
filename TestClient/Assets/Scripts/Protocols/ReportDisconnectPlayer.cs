using System.Collections.Generic;

public class ReportDisconnectPlayer : Protocol<ReportDisconnectPlayer, ReportDisconnectPlayer.Req, ReportDisconnectPlayer.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/ReportDisconnectPlayer"; }
	}

	[System.Serializable]
	public class Req
	{
		public int playerId;
	}

	[System.Serializable]
	public class Res
	{
	}
}
