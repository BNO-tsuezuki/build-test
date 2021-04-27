using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


namespace Http
{

	public abstract class HttpRequester<Req, Res>
	where Req : class, new()
	where Res : class
	{
		public abstract string Uri { get; }

		Req Request { get; set; }

		protected Res Response { get; set; }

		UnityWebRequest WebRequest { get; set; }

		public System.Func<Req, WWWForm> PreAction { private get; set; }
		public System.Action<Res> PostAction { private get; set; }

		public string RawResponseText { get; private set; }


		public HttpRequester()
		{
			PreAction = (req) => { return null; };
			PostAction = (res) => { };
		}

		~HttpRequester()
		{
			if (WebRequest != null)
			{
				WebRequest.Abort();
			}
		}

		public IEnumerator RequestCoroutine(string authToken)
		{
			Request = new Req();

			var form = PreAction(Request);

			WebRequest = UnityWebRequest.Post(Tester.Instance.HttpUrl + Uri, form);
			WebRequest.SetRequestHeader("Authorization", "Bearer " + authToken);

			if (form == null)
			{
				WebRequest.SetRequestHeader("Content-Type", "application/json");
				WebRequest.uploadHandler = new UploadHandlerRaw(
												Encoding.UTF8.GetBytes(
													JsonUtility.ToJson(Request)
												)
											);
			}

			yield return WebRequest.SendWebRequest();

			if (WebRequest.isNetworkError || WebRequest.isHttpError)
			{
				Debug.Log("(" + WebRequest.responseCode + ") :" + WebRequest.error);
			}
			else
			{
				var errRes = JsonUtility.FromJson<ErroResponse>(WebRequest.downloadHandler.text);
				if (errRes != null && errRes.error.code != null)
				{
					Debug.Log(errRes.error.code);
				}
				else
				{
					Debug.Log("OK:" + Uri);
					RawResponseText = WebRequest.downloadHandler.text;
					Response = JsonUtility.FromJson<Res>(RawResponseText);
					PostAction(Response);
				}
			}

			WebRequest = null;
		}

	}
}
