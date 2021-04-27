public class DioramaSave : Protocol<DioramaSave, DioramaSave.Req, DioramaSave.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Diorama/SaveSceneData"; }
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
