using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleServer : MonoBehaviour
{

	[SerializeField]
	string Account;

	[SerializeField]
	string Password;

	[SerializeField]
	string AuthToken;

	[SerializeField] Transform Requesters;

	[SerializeField] Text Name;
	[SerializeField] Button LoginButton;
	[SerializeField] Button LogoutButton;
	[SerializeField] Button MatchingEntryButton;
	[SerializeField] Button DisconnectPlayersButton;

	string IpAddr { get { return Password; } }

	List<int> AcceptedPlayers = new List<int>();

	Queue<IHttpRequester> RequestTaskQue { get; set; }

	public void AcceptClient( int playerId, string ipAddr, int port, string joinPassword)
	{
		if (IpAddr != ipAddr) return;

		var reportAcceptPlayer = ReportAcceptPlayer.Create(Requesters);
		reportAcceptPlayer.PreAction = (req) =>
		{
			req.joinPassword = joinPassword;
			req.playerId = playerId;
			return null;
		};
		reportAcceptPlayer.PostAction = (res) =>
		{
			LogDisplay.Instance.Push("allowed:" + res.allowed + "side:" + res.side);
			if (res.allowed) AcceptedPlayers.Add(playerId);
		};
		RequestTaskQue.Enqueue(reportAcceptPlayer);
	}

	private IEnumerator Start()
	{
		this.name = Name.text = IpAddr;

		RequestTaskQue = new Queue<IHttpRequester>();

		LoginButton.onClick.AddListener(() =>
		{
			System.Action handShakeSetup = () => { };
			handShakeSetup = () =>
			{
				var handShake = HandShake.Create(Requesters);
				handShake.PostAction = (res) =>
				{
					LogDisplay.Instance.Push("["+res.pushCode+"]");

					if( res.pushCode == typeof(HandShake.ChangeBattlePhase).Name)
					{
						var changeBattlePhase = handShake.ConvertResponse<HandShake.ChangeBattlePhase>();
						LogDisplay.Instance.Push(changeBattlePhase.phase);
					}

					if (res.pushCode == typeof(HandShake.Close).Name)
					{
						var close = handShake.ConvertResponse<HandShake.Close>();
						LogDisplay.Instance.Push(close.pushCode + " : " + close.reason);
						return;
					}

					if( res.pushCode == typeof(HandShake.ExecCommand).Name)
					{
						var execCommand = handShake.ConvertResponse<HandShake.ExecCommand>();
						LogDisplay.Instance.Push(execCommand.command);
					}

					if( res.pushCode == typeof(HandShake.MatchInfo).Name)
					{
						var matchInfo = handShake.ConvertResponse<HandShake.MatchInfo>();
						LogDisplay.Instance.Push("matchId:"+matchInfo.matchId + "/matchType: " + matchInfo.matchType);
					}

					handShakeSetup();
				};
				StartCoroutine(handShake.RequestCoroutine(AuthToken));
			};

			var authLogin = AuthLogin.Create(Requesters);
			authLogin.PreAction = (req) =>
			{
				req.account = Account;
				req.password = Password;
				return null;
			};
			authLogin.PostAction = (res) =>
			{
				AuthToken = res.token;
				handShakeSetup();
			};
			RequestTaskQue.Enqueue(authLogin);

		});

		LogoutButton.onClick.AddListener(() =>
		{
			var authLogout = AuthLogout.Create(Requesters);
			RequestTaskQue.Enqueue(authLogout);
		});

		MatchingEntryButton.onClick.AddListener(() =>
		{
			var matchingEntry = MatchingEntryBattleServer.Create(Requesters);
			matchingEntry.PreAction = (req) =>
			{
				req.ipAddr = IpAddr;
				req.port = 12345;
				req.rule = "PointCapture";
				req.mapId = "Lv909";
				req.label = "ビルドラベルゥ";
				req.description = "おれサーバー";
				req.autoMatchmakeTarget = true;
				req.serverName = "サバ太郎";
				req.region = "ap-east-1";
				req.owner = "yoshimura";
				return null;
			};
			RequestTaskQue.Enqueue(matchingEntry);
		});

		DisconnectPlayersButton.onClick.AddListener(() =>
		{
			AcceptedPlayers.ForEach(playerId =>
			{
				var reportDisconnectPlayer = ReportDisconnectPlayer.Create(Requesters);
				reportDisconnectPlayer.PreAction = (req) =>
				{
					req.playerId = playerId;
					return null;
				};
				RequestTaskQue.Enqueue(reportDisconnectPlayer);
			});
			AcceptedPlayers.Clear();
		});


		while (true)
		{
			while (RequestTaskQue.Count != 0)
			{
				var requester = RequestTaskQue.Dequeue();

				yield return requester.RequestCoroutine(AuthToken);
			}

			yield return new WaitForSeconds(1);
		}
	}

}
