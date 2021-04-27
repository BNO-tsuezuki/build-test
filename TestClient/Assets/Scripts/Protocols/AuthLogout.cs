public class AuthLogout : Protocol<AuthLogout, AuthLogout.Req, AuthLogout.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Auth/Logout"; }
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
