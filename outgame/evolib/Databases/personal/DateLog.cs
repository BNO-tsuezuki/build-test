using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class DateLog
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long playerId { get; set; }

		[Required]
		public DateTime OnlineStamp { get; set; }

		[Required]
		public DateTime FriendRequestPageLastView { get; set; }



		public DateLog(long playerId)
		{
			this.playerId = playerId;
			OnlineStamp = DateTime.MinValue;
			FriendRequestPageLastView = DateTime.MinValue;
		}

	}
}
