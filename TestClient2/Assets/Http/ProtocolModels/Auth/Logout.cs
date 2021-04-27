using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Http.ProtocolModels.Auth
{
	public class Logout : HttpRequester<Login.Req, Login.Res>
	{
		override public string Uri
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
}
