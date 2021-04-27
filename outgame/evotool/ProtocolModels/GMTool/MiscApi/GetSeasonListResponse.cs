using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.MiscApi
{
    public class GetSeasonListResponse
    {
        public class Season
        {
            public int seasonNo { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
        }

        public IList<Season> seasons { get; set; }
    }
}
