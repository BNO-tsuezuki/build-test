using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerCareerRecordResponse
    {
        public class CareerRecord
        {
            public long careerRecordId { get; set; }

            public string recordItemId { get; set; }

            public string displayNameJapanese { get; set; }

            public string displayNameEnglish { get; set; }

            public double value { get; set; }

            public int numForAverage { get; set; }
        }

        public IList<CareerRecord> careerRecords { get; set; }
    }
}
