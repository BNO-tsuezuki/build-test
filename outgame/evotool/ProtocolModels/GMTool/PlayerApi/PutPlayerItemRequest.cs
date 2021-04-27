using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class PutPlayerItemRequest
    {
        public class Item
        {
            [Required]
            public string itemId { get; set; }
            [Required]
            public bool owned { get; set; }
        }

        [Required]
        public List<Item> items { get; set; }
    }
}
