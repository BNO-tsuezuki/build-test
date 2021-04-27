using System;
using System.Collections.Generic;


namespace evolib.Services.MasterData
{
	public interface IItem
	{
		string itemId { get; }

		string displayName { get; }

		evolib.Item.Type itemType { get; }

        evolib.Item.RankType rankType { get; }

        bool isDefaultSetting { get; }
	}

    public class Item : IItem
	{
		public string itemId { get; set; }

		public string displayName { get; set; }

		public evolib.Item.Type itemType { get; set; }

        public evolib.Item.RankType rankType { get; set; }

        public bool isDefaultSetting { get; set;  }


		public static evolib.Item.Type ItemTypeFromRowStructName(string rowStructName)
		{
			switch(rowStructName)
			{
				case "EvoPlayerIconRow":	return evolib.Item.Type.PlayerIcon;
				case "EvoVoicePackRow":		return evolib.Item.Type.VoicePack;
				case "EvoOrnamentRow":		return evolib.Item.Type.Ornament;
				case "EvoBodySkinRow":		return evolib.Item.Type.BodySkin;
				case "EvoWeaponSkinRow":	return evolib.Item.Type.WeaponSkin;
				case "EvoMVPSequenceRow":	return evolib.Item.Type.MvpCelebration;
				case "EvoStampRow":			return evolib.Item.Type.Stamp;
				case "EvoEmotionRow":		return evolib.Item.Type.Emotion;
				case "EvoCharacterRow":		return evolib.Item.Type.MobileSuit;
				default: return evolib.Item.Type.Unknown;
			}
		}
	}
}
