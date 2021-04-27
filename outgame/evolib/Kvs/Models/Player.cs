using System;
using System.Threading.Tasks;
using evolib.Databases.personal;


namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class Player : KvsHashModel<Player.INFC, Player.IMPL>
	{
		public Player(long id) : base(id.ToString()) { playerId = id; }

		public long playerId { get; set; }


		public interface INFC
		{
			string sessionId { get; set; }

			int privilegeLevel { get; set; }

			MatchingArea matchingArea { get; set; }



			bool validated { get; set; }

			string playerName { get; set; }

			string playerIconItemId { get; set; }

			float battleRating { get; set; }

			bool pretendOffline { get; set; }

			PlayerInformation.OpenType openType { get; set; }

            int playerLevel { get; set; }

            UInt64 exp { get; set; }

			UInt64 nextLevelExp { get; set; }

			UInt64 packageVersion { get; set; }
		}

		public class IMPL : INFC
		{
			public string sessionId { get; set; }

			public int privilegeLevel { get; set; }

			public MatchingArea matchingArea { get; set; }



			public bool validated { get; set; }

			public string playerName { get; set; }

			public string playerIconItemId { get; set; }

			public float battleRating { get; set; }

			public bool pretendOffline { get; set; }

			public PlayerInformation.OpenType openType { get; set; }

            public int playerLevel { get; set; }

            public UInt64 exp { get; set; }

            public UInt64 nextLevelExp { get; set; }

			public UInt64 packageVersion { get; set; }
		}

		public async Task Invalidate()
		{
			Model.validated = false;
			await SaveAsync();
		}

		public async Task<bool> Validate( PersonalDBShardManager pdbsm )
		{
			await FetchAsync();

			if (!Model.validated)
			{
				var db = pdbsm.PersonalDBContext(playerId);

				var basic = await db.PlayerBasicInformations.FindAsync(playerId);
				if (basic == null)
				{
					return false;
				}

				Model.validated = true;
				Model.playerName = basic.playerName;
				Model.playerIconItemId = basic.playerIconItemId;
				Model.pretendOffline = basic.pretendOffline;
				Model.openType = basic.openType;

				var playerPass = await db.BattlePasses.FindAsync(playerId, (int)BattlePass.PlayerLevelPassId);
				if (playerPass != null)
				{
					Model.playerLevel = playerPass.level;
					Model.exp = playerPass.levelExp;
					Model.nextLevelExp = playerPass.nextExp;
				}


				var battle = await db.PlayerBattleInformations.FindAsync(playerId);
				if (battle != null)
				{
					Model.battleRating = battle.rating;
				}
				else
				{
					Model.battleRating = Battle.InitialRating;
				}
				await SaveAsync();
			}
			return true;
		}
	}
}
