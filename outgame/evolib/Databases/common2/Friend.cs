using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common2
{
	public class Friend
	{
		public long playerIdL { get; set; }

		public long playerIdR { get; set; }

		public DateTime timeStamp { get; set; }


		public static Friend Create(long player1, long player2)
		{
			if (player1 == player2) throw new Exception("Failed: Same playerId!");
			return new Friend()
			{
				playerIdL = Math.Min(player1, player2),
				playerIdR = Math.Max(player1, player2),
				timeStamp = DateTime.UtcNow,
			};
		}
	}
}
