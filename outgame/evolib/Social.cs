using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public static class Social
    {
		public static int MaxRecentPlayersCnt = 99;

		public static int MaxFriendRequestsCnt = 99;
		public static int MaxFriendCnt = 99;
		public static int MaxMutePlayerCnt = 99;


        //public static uint PlayerNo( long playerId, string playerName )
        //{
        //	var str = playerName + "#%$" + playerId + "bandainamco-ol";

        //	return Util.Hasher.ToUint(str, 90000) + 10000;
        //}

        public enum ResponseType
        {
            Admit = 0,
            Deny,
        }
    }
}
