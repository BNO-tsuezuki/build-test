using System;
using System.Collections.Generic;

namespace evolib.Services.MasterData
{
	public interface IMobileSuit
	{
		string mobileSuitId { get; }
		string itemId { get; }
		bool availabled { get; }
		bool CheckEnabledItemId(evolib.Item.Type itemType, string itemId);
		string DefaultItemId(evolib.Item.Type itemType);
		string EmptyItemId(evolib.Item.Type itemType);
	}

	public class MobileSuit : IMobileSuit
	{
		public string mobileSuitId { get; set; }

		public string itemId { get; set; }

		public bool availabled { get; set; }

		public bool CheckEnabledItemId(evolib.Item.Type itemType, string itemId)
		{
			if( Choices[itemType].ContainsKey(itemId) )
			{
				return true;
			}

			return false;
		}

		public string DefaultItemId(evolib.Item.Type itemType)
		{
			return DefaultSettings[itemType];
		}

		public string EmptyItemId(evolib.Item.Type itemType)
		{
			return EmptyChoices[itemType];
		}

		Dictionary<evolib.Item.Type, Dictionary<string, int>> Choices = new Dictionary<evolib.Item.Type, Dictionary<string, int>>();
		Dictionary<evolib.Item.Type, string> DefaultSettings = new Dictionary<evolib.Item.Type, string>();
		Dictionary<evolib.Item.Type, string> EmptyChoices = new Dictionary<evolib.Item.Type, string>();

		public MobileSuit()
		{
			foreach (evolib.Item.Type itemType in Enum.GetValues(typeof(evolib.Item.Type)))
			{
				Choices[itemType] = new Dictionary<string, int>();
				DefaultSettings[itemType] = "";
				EmptyChoices[itemType] = "";
			}
		}


		public void AddChoices(evolib.Item.Type itemType, string itemId, bool isDefaultSetting, bool isEmpty )
		{
			Choices[itemType][itemId] = 12345;

			if (isDefaultSetting)
			{
				DefaultSettings[itemType] = itemId;
			}
			if(isEmpty)
			{
				EmptyChoices[itemType] = itemId;
			}
		}
	}
}
