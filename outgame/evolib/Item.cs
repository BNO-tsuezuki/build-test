using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
    public class Item
    {
		public enum Type
		{
			Unknown = 0,
			PlayerIcon,
			VoicePack,
			Ornament,
			BodySkin,
			WeaponSkin,
			MvpCelebration,
			Stamp,
			Emotion,
			MobileSuit,
		}

        public enum RankType
        {
            Normal = 0,
            Bronze,
            Silver,
            Gold,
            Platinum,
        }

        public enum ObtainedWay
		{
			Game = 0,
			Tool = 2,
		}

        public class OpenItem
        {
            public string itemId { get; set; }
            public bool close { get; set; }
        }
    }
}
