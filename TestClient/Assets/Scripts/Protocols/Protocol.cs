using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public interface IHttpRequester
{
	string Uri { get; }

	IEnumerator RequestCoroutine(string authToken);
}

public abstract class Protocol<T,Req,Res> : MonoBehaviour
	where T: Protocol<T, Req, Res>, IHttpRequester 
	where Req:new()
{
	Req Request { get; set; }

	Res Response { get; set; }

	string ResponseTextRaw = "";
	public ResRes ConvertResponse<ResRes>() { return JsonUtility.FromJson<ResRes>(ResponseTextRaw); }

	public System.Func<Req,WWWForm> PreAction { private get; set; }
	public System.Action<Res> PostAction { private get; set; }

	UnityWebRequest WebRequest { get; set; }

	public static T Create( Transform parent )
	{
		var go = new GameObject();
		go.transform.parent = parent;
		go.name = "" + DateTime.Now + "-" + typeof(T).Name;

		return go.AddComponent<T>();
	}

	void Awake()
	{
		PreAction = (req) => { return null; };
		PostAction = (res) => { };
	}

	private void OnDestroy()
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

		var uri = Main.Instance.CurrentServerInfo.HttpUrl + ((T)this).Uri;

		WebRequest = UnityWebRequest.Post(uri, form);
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
			LogDisplay.Instance.Push("(" + WebRequest.responseCode + ") :" + WebRequest.error);
		}
		else
		{
			var errRes = JsonUtility.FromJson<ErroResponse>(WebRequest.downloadHandler.text);
			if ( errRes!=null && errRes.error.code != null)
			{
				LogDisplay.Instance.Push(errRes.error.code);
			}
			else
			{
				LogDisplay.Instance.Push("OK:"+ ((T)this).Uri);
				ResponseTextRaw = WebRequest.downloadHandler.text;
				Response = JsonUtility.FromJson<Res>(ResponseTextRaw);
				PostAction(Response);
			}
		}

		WebRequest = null;
		GameObject.Destroy(this.gameObject);
	}

}
