using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Services.MasterData
{
	public interface IBattleRatingTier
	{
		string Id { get; }
		PlayerInformation.BattleRatingTierType tierType { get; }
		int startRange { get; }
		int endRange { get; }
		bool isUnrank { get; }
	}

	public class BattleRatingTier : IBattleRatingTier
    {
		public string Id { get; private set; }
		public PlayerInformation.BattleRatingTierType tierType { get; private set; }
		public int startRange { get; private set; }
		public int endRange { get; private set; }
		public bool isUnrank { get; private set; }

		//----
		public BattleRatingTier(string battleRatingTierId,
								PlayerInformation.BattleRatingTierType tierType,
								int startRange, int endRange, bool isUnrank)
		{
			this.Id = battleRatingTierId;
			this.tierType = tierType;
			this.startRange = startRange;
			this.endRange = endRange;
			this.isUnrank = isUnrank;
		}
	}
}
