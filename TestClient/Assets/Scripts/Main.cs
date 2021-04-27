using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : SingletonMonoBehaviour<Main>
{
	public class ServerInfo
	{
		public string ipAddr { get; set; }
		public int port { get; set; }

		public string HttpUrl { get { return "http://" + ipAddr + ":" + port; } }
		public string WebSocketUrl { get { return "ws://" + ipAddr + ":" + port + "/notify"; } }
	}

	List<ServerInfo> serverInfoList = new List<ServerInfo>()
	{
		new ServerInfo(){ ipAddr="localhost",			port=52345 },
		new ServerInfo(){ ipAddr="takatenjin.bnug.jp",	port=52345 },
		new ServerInfo(){ ipAddr="odani.bnug.jp",		port=52345 },
	};

	public enum TargetEnvironment
	{
		Development = 0,
		Takatenjin,
		Odani,
	}
	public TargetEnvironment targetEnviroment;

	public ServerInfo CurrentServerInfo { get { return serverInfoList[(int)targetEnviroment]; } }

}
