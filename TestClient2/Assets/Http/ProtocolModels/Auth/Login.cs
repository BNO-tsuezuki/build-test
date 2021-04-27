using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Http.ProtocolModels.Auth
{
	public class Login : HttpRequester<Login.Req, Login.Res>
	{
		override public string Uri
		{
			get { return "/api/Auth/Login"; }
		}


		[System.Serializable]
		public class Req
		{
			public string account;
			public string password;
		}

		[System.Serializable]
		public class Res
		{
			public string token;
			public int playerId;
			public int initialLevel;
		}
	}
}
