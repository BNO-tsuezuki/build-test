using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour {

	[SerializeField]
	string Account;

	[SerializeField]
	string Password;

	[SerializeField]
	string AuthToken;

	[SerializeField]
	string PlayerName;

	[SerializeField]
	int PlayerId;


	[SerializeField] Transform Requesters;

	[SerializeField] Text Name;
	[SerializeField] Button LoginButton;
	[SerializeField] Button SetFirstOnetimeButton;
	[SerializeField] Button LogoutButton;
	[SerializeField] Button MatchingEntryButton;
	[SerializeField] Toggle CasualMatch;
	[SerializeField] Toggle RankMatch;
	[SerializeField] Button MatchingCancelButton;
	[SerializeField] Button GetBattlesListButton;
	[SerializeField] Dropdown BattlesListDropdown;
	[SerializeField] Button RequestJoinBattleButton;
	[SerializeField] Button DioramaSaveButton;
	[SerializeField] Button DioramaUploadButton;


	Queue<IHttpRequester> RequestTaskQue { get; set; }

	private IEnumerator Start()
	{
		this.name = Name.text = Password;

		RequestTaskQue = new Queue<IHttpRequester>();

		LoginButton.onClick.AddListener(() =>
		{
			System.Action handShakeSetup = () => { };
			handShakeSetup = () =>
			{
				var handShake = HandShake.Create(Requesters);
				handShake.PostAction = (res) =>
				{
					var masterDataVersion = "";
					if( res.masterDataVersion != null )
					{
						if (0 < res.masterDataVersion.Length) masterDataVersion += res.masterDataVersion[0] + "-";
						if (1 < res.masterDataVersion.Length) masterDataVersion += "-" + res.masterDataVersion[1];
						if (2 < res.masterDataVersion.Length) masterDataVersion += "-" + res.masterDataVersion[2];
					}

					var noticeCodes = "";
					foreach( var code in res.opsNoticeCodes)
					{
						noticeCodes += "[" + code + "]";
					}

					var disabledMS = "";
					foreach( var ms in res.disabledMobileSuits)
					{
						disabledMS += "{" + ms +"}";
					}

					LogDisplay.Instance.Push("["
						+ res.pushCode
						+ "(" + masterDataVersion + ")"
						+ " matchmake:" + res.enabledMatchmake
						+ noticeCodes
						+ disabledMS
					+ "]");

					if (res.pushCode == typeof(HandShake.JoinBattle).Name)
					{
						var joinBattle = handShake.ConvertResponse<HandShake.JoinBattle>();
						
						var battleServer = GameObject.Find(joinBattle.ipAddr).GetComponent<BattleServer>();
						battleServer.AcceptClient( PlayerId, joinBattle.ipAddr, joinBattle.port, joinBattle.joinPassword );

						LogDisplay.Instance.Push(
							joinBattle.ipAddr + ":" +
							joinBattle.port + ":" +
							joinBattle.joinPassword);

						Debug.Log("token:" + joinBattle.token);
						Debug.Log("encryptionKey:" + joinBattle.newEncryptionKey);
					}

					if (res.pushCode == typeof(HandShake.Close).Name)
					{
						var close = handShake.ConvertResponse<HandShake.Close>();
						LogDisplay.Instance.Push(close.pushCode + " : " + close.reason);
						return;
					}

					if (res.pushCode == typeof(HandShake.Chat).Name)
					{
						var chat = handShake.ConvertResponse<HandShake.Chat>();
						LogDisplay.Instance.Push(chat.playerId + ":" + chat.playerName + " : " + chat.text);
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
				req.packageVersion = new int[]{ 1,1,1,1, };
				return null;
			};
			authLogin.PostAction = (res) =>
			{
				AuthToken = res.token;
				PlayerId = res.playerId;
				LogDisplay.Instance.Push("initialLevel: " + res.initialLevel);
				handShakeSetup();
			};
			RequestTaskQue.Enqueue(authLogin);
			
		});

		SetFirstOnetimeButton.onClick.AddListener(() =>
		{
			var setFirstOnetime = SetFirstOnetime.Create(Requesters);
			setFirstOnetime.PreAction = (req) =>
			{
				req.playerName = PlayerName;
				return null;
			};
			RequestTaskQue.Enqueue(setFirstOnetime);
		});

		LogoutButton.onClick.AddListener(() =>
		{
			var authLogout = AuthLogout.Create(Requesters);
			RequestTaskQue.Enqueue(authLogout);
		});

		MatchingEntryButton.onClick.AddListener(() =>
		{
			var matchingEntry = MatchingEntryPlayer.Create(Requesters);
			matchingEntry.PreAction = (req) =>
			{
				req.matchType = (CasualMatch.isOn) ? 0 : 1;
				return null;
			};
			RequestTaskQue.Enqueue(matchingEntry);
		});

		MatchingCancelButton.onClick.AddListener(() =>
		{
			var matchingCancel = MatchingCancelPlayer.Create(Requesters);
			RequestTaskQue.Enqueue(matchingCancel);
		});

		GetBattlesListButton.onClick.AddListener(() =>
		{
			var getBattlesList = GetBattlesList.Create(Requesters);
			getBattlesList.PostAction = (res) =>
			{
				var list = new List<string>();
				res.battlesList.ForEach(bs => list.Add(bs.battleId));
				BattlesListDropdown.ClearOptions();
				BattlesListDropdown.AddOptions(list);
			};

			RequestTaskQue.Enqueue(getBattlesList);
		});

		RequestJoinBattleButton.onClick.AddListener(() =>
		{
			var idx = BattlesListDropdown.value;
			var options = BattlesListDropdown.options;
			if (idx < options.Count)
			{
				var requestJoinBattle = RequestJoinBattle.Create(Requesters);
				requestJoinBattle.PreAction = (req) =>
				{
					req.battleId = options[idx].text;
					return null;
				};

				RequestTaskQue.Enqueue(requestJoinBattle);
			}
		});

		DioramaSaveButton.onClick.AddListener(() =>
		{
			var dioramaSave = DioramaSave.Create(Requesters);
			dioramaSave.PreAction = (req) =>
			{
				var fs = System.IO.File.Open("C:/Users/t-yoshimura/Desktop/img_2_m.jpg",
												System.IO.FileMode.Open,
												System.IO.FileAccess.Read);
				var bytes = new byte[fs.Length];
				fs.Read(bytes, 0, bytes.Length);
				fs.Close();

				var form = new WWWForm();
				form.AddBinaryData("sceneData", bytes, "filename1", "application/octet-stream");
				form.AddField("index", 3);
				form.AddField("hashCode", Random.rotation.ToString("F4"));
				return form;
			};
			RequestTaskQue.Enqueue(dioramaSave);
		});

		DioramaUploadButton.onClick.AddListener(() =>
		{
			var dioramaUpload = DioramaUpload.Create(Requesters);
			dioramaUpload.PreAction = (req) =>
			{
				var form = new WWWForm();
				{
					var fs = System.IO.File.Open("C:/Users/t-yoshimura/Desktop/kinniku_ude.png",
													System.IO.FileMode.Open,
													System.IO.FileAccess.Read);
					var bytes = new byte[fs.Length];
					fs.Read(bytes, 0, bytes.Length);
					fs.Close();

					form.AddBinaryData("sceneData", bytes, "filename1", "application/octet-stream");
				}
				{
					var fs = System.IO.File.Open("C:/Users/t-yoshimura/Desktop/diet_before_man.png",
													System.IO.FileMode.Open,
													System.IO.FileAccess.Read);
					var bytes = new byte[fs.Length];
					fs.Read(bytes, 0, bytes.Length);
					fs.Close();

					form.AddBinaryData("visual", bytes, "filename2", "application/octet-stream");
				}

				form.AddField("hashCode", Random.rotation.ToString("F4"));

				return form;
			};
			RequestTaskQue.Enqueue(dioramaUpload);
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
