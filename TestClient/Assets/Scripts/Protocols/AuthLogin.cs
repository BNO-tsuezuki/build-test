public class AuthLogin : Protocol<AuthLogin, AuthLogin.Req, AuthLogin.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Auth/Login"; }
	}

	[System.Serializable]
	public class Req
	{
		public string account;
		public string password;
		public int[] packageVersion;
	}

	[System.Serializable]
	public class Res
	{
		public string token;
		public int playerId;
		public int initialLevel;
	}
}
