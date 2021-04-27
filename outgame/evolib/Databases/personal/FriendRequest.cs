using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class FriendRequest
	{
		public long playerIdDst { get; set; }

		public long playerIdSrc { get; set; }

		public DateTime timeStamp { get; set; }
	}
}
