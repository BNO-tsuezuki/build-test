using System.Collections.Generic;

public class ReportAcceptPlayer : Protocol<ReportAcceptPlayer, ReportAcceptPlayer.Req, ReportAcceptPlayer.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/ReportAcceptPlayer"; }
	}

	[System.Serializable]
	public class Req
	{
		public int playerId;

		public string joinPassword;
	}

	[System.Serializable]
	public class Res
	{
		public bool allowed;

		public int side;
	}
}
