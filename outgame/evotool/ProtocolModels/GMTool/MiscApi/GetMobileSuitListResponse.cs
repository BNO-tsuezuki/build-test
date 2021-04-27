using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.MiscApi
{
    public class GetMobileSuitListResponse
    {
        public class MobileSuit
        {
            public string mobileSuitId { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
        }

        public IList<MobileSuit> mobileSuits { get; set; }
    }
}
