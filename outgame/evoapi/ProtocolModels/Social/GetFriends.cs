using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Social
{
    public class GetFriends
    {
		public class Request
		{
		}


		public class Player
		{
			public long playerId { get; set; }
		}

		public class FriendPlayer : Player
		{
			public DateTime lastLogin { get; set; }
			public bool favorite { get; set; }
			public string onlineState { get; set; }
		}

		public class RequestPlayer : Player
		{
			public DateTime applicationDate { get; set; }
		}

		public class MutePlayer : Player
		{
			public bool text { get; set; }
			public bool voice { get; set; }
		}

		public class Lists
		{
			public List<FriendPlayer> Friends { get; set; }
			public List<RequestPlayer> Requests { get; set; }
			public List<MutePlayer> MutePlayers { get; set; }
		}

		public class Response
		{
			public DateTime FriendRequestPageLastView { get; set; }

			public Lists lists { get; set; }
		}
	}
}
