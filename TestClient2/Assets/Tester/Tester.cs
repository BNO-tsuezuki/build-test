using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : SingletonMonoBehaviour<Tester>
{
	public Player PlayerPrefab;
	public List<Player> PlayerList = new List<Player>();

	public enum TargetEnvironment
	{
		Development = 0,
		DevEnv,
		DevEnvBeta,
	}
	public TargetEnvironment targetEnviroment;


	public enum TestType{
		Keep = 0,
		SimpleLoginLogout = 1,
	}

	public TestType testType;

	private void Start()
	{
		var animator = GetComponent<Animator>();
		animator.SetInteger("testType", (int)testType);
	}

	public string HttpUrl {
		get
		{
			switch (targetEnviroment)
			{
				case TargetEnvironment.Development: return "http://localhost:52345";
				case TargetEnvironment.DevEnv: return "http://takatenjin.devenv.jpop.bnug.jp:52345";
				case TargetEnvironment.DevEnvBeta: return "http://18.182.87.121:52345";
			}
			return "http://hogehogehoge";
		}
	}

}
