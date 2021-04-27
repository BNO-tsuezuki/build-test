using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.personal
{
	public class FavoriteFriend
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public long playerId { get; set; }

		public long favoritePlayerId { get; set; }
	}
}
