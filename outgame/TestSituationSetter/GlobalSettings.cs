using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TestSituationSetter
{
	public enum Env
	{
		local,
		takatenjin,
		odani,
		ingrid,
		kasugayama,
	}

	public static class GlobalSettings
	{
		public static FileVersionInfo FileVersionInfo { get; set; }

		public static string MyAccount { get; set; }
		public static string MyPassword { get; set; }
		public static long MyPlayerId { get; set; }

		public static Env TargetEnv { get; set; }
		public static string TargetUrl { get
			{
				switch (TargetEnv)
				{
				//case Env.local:			return "http://localhost:52345";
				case Env.takatenjin:	return "https://takatenjin.bnug.jp:52346";
				case Env.odani:			return "https://odani.bnug.jp:52346";
				case Env.ingrid:		return "https://ingrid.bnug.jp:52346";
				case Env.kasugayama:	return "https://kasugayama.bnug.jp:52346";
				}
				return "http://localhost:52345";
			}
		}
	}
}
