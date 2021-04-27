using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerItemResponse
    {
        public class Item
        {
            public string itemId { get; set; }
            public string itemType { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
            public bool owned { get; set; }
            public bool isDefault { get; set; }
        }

        public IList<Item> items { get; set; }
    }
}
