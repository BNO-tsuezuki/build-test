using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.UnitApi
{
    public class GetUnitResponse
    {
        public class Unit
        {
            public string mobileSuitId { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
            public bool isEnabledOnMasterData { get; set; }
            public bool isEnabledOnGmTool { get; set; }
            public bool isAvailable { get; set; }
        }

        public IList<Unit> units { get; set; }
    }
}
