using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;

public class WebSocketSession : MonoBehaviour
{
	WebSocket CurrentSocket { get; set; }

	WebSocketState State {
		get
		{
			if (CurrentSocket == null) return WebSocketState.Closed;
			return CurrentSocket.ReadyState;
		}
	}

	[System.Serializable]
	public class PushData
	{
		public string PushCode;
	}
	Queue<PushData> PushDataQue = new Queue<PushData>();


	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	public void Connect(string uri, string jwt)
	{
		StartCoroutine(ConnectCoroutine(uri,jwt));
	}

	IEnumerator ConnectCoroutine(string uri, string jwt)
	{
		using (var sock = new WebSocket(uri))
		{
			sock.CustomHeaders = new Dictionary<string, string>
			{
				{ "Authorization", "Bearer " + jwt }
			};

			sock.OnOpen += (sender, e) =>
			{
				LogDisplay.Instance.Push("WebSocket connection has been established.");
			};

			sock.OnClose += (sender, e) =>
			{
				LogDisplay.Instance.Push("WebSocket connection has been closed. Reason=" + e.Reason);
			};

			sock.OnMessage += (sender, e) =>
			{
				if (sock != CurrentSocket) return;

				Debug.Log(e.Data);
				PushDataQue.Enqueue(JsonUtility.FromJson<PushData>(e.Data));
			};

			sock.OnError += (sender, e) =>
			{

			};

			if (CurrentSocket != null)
			{
				PushDataQue.Clear();
			}
			CurrentSocket = sock;

			//-----------------------
			sock.ConnectAsync();
			while (sock.ReadyState == WebSocketState.Connecting)
			{
				yield return new WaitForSeconds(1);
			}

			while (sock.ReadyState == WebSocketState.Open)
			{
				yield return new WaitForSeconds(1);
			}
		}
	}
}
