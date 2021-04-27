using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
	public class PlayerInformation
	{
		public class InitialLevelFlg
		{
			public static int Name		= (1 << 0);
			public static int Tutorial	= (1 << 1);
		}

        public enum TutorialType
        {
            Tutorial_None = 0,
            Tutorial_Walk,
            Tutorial_BoostStep,
            Tutorial_BoostCruise,
            Tutorial_Jump,
            Tutorial_Hovering,
            Tutorial_MainWeapon,
            Tutorial_SubAction1,
            Tutorial_SubAction3,
            Tutorial_Pin,
            Tutorial_PresetChat,
            Tutorial_Recovery,
            Tutorial_GManeuver,
            Tutorial_BombPlace,
            Tutorial_BombDefuse,
            Tutorial_FOBConquering,
            Tutorial_End,
        }

        public enum OpenType
		{
			Private = 0,
			FriendOnly,
			Public,
		}

		public enum BattleRatingTierType
		{
			Unrank=0,
			Bronze,
			Silver,
			Gold,
			Platinum,
			Diamond,
			Master,
			Grandmaster,
			Newtype,
		}
	}
}
