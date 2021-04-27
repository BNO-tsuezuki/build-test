using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public static class Chat
    {
		public enum Type
		{
			Individual  = 0,
			PremadeGroup,
			BattleSide,
			BattleMatch,
			System,
			GameMaster,
		}

		public enum Const
		{
			MaxStringLength = 140,
		}
	}
}
