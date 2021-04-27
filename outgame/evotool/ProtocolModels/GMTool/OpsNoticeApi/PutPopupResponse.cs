using System;

namespace evotool.ProtocolModels.GMTool.OpsNoticeApi
{
    public class PutPopupResponse
    {
        public long Id { get; set; }

        public ulong Target { get; set; }

        public DateTime updateDate { get; set; }
        public int version { get; set; }

        public bool release { get; set; }

        public string memo { get; set; }

        public DateTime beginDate { get; set; }
        public DateTime endDate { get; set; }

        public bool enabledEnglish { get; set; }
        public string msgEnglish { get; set; }

        public bool enabledFrench { get; set; }
        public string msgFrench { get; set; }

        public bool enabledGerman { get; set; }
        public string msgGerman { get; set; }

        public bool enabledJapanese { get; set; }
        public string msgJapanese { get; set; }

        public string titleEnglish { get; set; }

        public string titleFrench { get; set; }

        public string titleGerman { get; set; }

        public string titleJapanese { get; set; }
    }
}
