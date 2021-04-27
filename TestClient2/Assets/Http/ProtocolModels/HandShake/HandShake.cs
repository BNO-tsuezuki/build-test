using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Http.ProtocolModels.HandShake
{
	public class HandShake : HttpRequester<HandShake.Req, HandShake.Res>
	{
		override public string Uri
		{
			get { return "/api/HandShake"; }
		}

		[System.Serializable]
		public class Req
		{
		}

		[System.Serializable]
		public class Res
		{
			public string pushCode;
		}

		[System.Serializable]
		public class Unauthorized : Res
		{
		}
		[System.Serializable]
		public class Close : Res
		{
			public string reason;
		}
		[System.Serializable]
		public class Poke : Res
		{
		}

		public T Remap<T>()
				where T : Res
		{
			if (typeof(T).Name == Response.pushCode)
			{
				return JsonUtility.FromJson<T>(this.RawResponseText);
			}

			return null;
		}

	}
}
