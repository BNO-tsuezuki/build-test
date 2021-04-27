using System.Collections.Generic;

public class GetBattlesList : Protocol<GetBattlesList, GetBattlesList.Req, GetBattlesList.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Matching/GetBattlesList"; }
	}

	[System.Serializable]
	public class Req
	{
	}

	[System.Serializable]
	public class Res
	{
		[System.Serializable]
		public class Battle
		{
			public string battleId;
			public string name;
			public string rule;
			public string mapId;
		}
		public List<Battle> battlesList;
	}

}
