using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public class Battle
    {
		public enum Phase
		{
			START,
			ABORT,
			RESULT,
			FINISH,
		}

		public enum Result
		{
			VICTORY = 0,
			DEFEAT,
			DRAW,
			OTHER,
		}

		public enum Side
		{
			Unknown = -1,
			Earthnoid = 0,
			Spacenoid,
			Other,
		}

		public enum MatchType
		{
			Casual = 0,
			Rank,
			Custom,
            Tournament,//運営主催の大会
        }

		public static int SidePlayersNum { get { return 6; } }
		public static int MatchPlayersNum { get { return SidePlayersNum*2; } }

		public static float InitialRating { get { return 2000; } }
        
    }

	public static class SideExtensions
	{
		public static Battle.Side Opponet(this Battle.Side side)
		{
			if (side == Battle.Side.Earthnoid) return Battle.Side.Spacenoid;
			if (side == Battle.Side.Spacenoid) return Battle.Side.Earthnoid;
			return Battle.Side.Other;
		}
	}

}
