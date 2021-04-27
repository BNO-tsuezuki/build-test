using System;
using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.GMTool.OpsNoticeApi
{
    public class PostPopupRequest
    {
        // todo: PutPopupRequestも同様に

        [Required]
        public ulong target { get; set; }

        [Required]
        public bool release { get; set; }

        // todo: maxlength
        [MaxLength(255)]
        public string memo { get; set; }

        // todo: begin < end
        [Required]
        public DateTime beginDate { get; set; }

        [Required]
        public DateTime endDate { get; set; }

        // todo: maxlength
        // todo: maxlines
        // todo: 文字種？
        // todo: CBT後 テキスト装飾

        [Required]
        public bool enabledEnglish { get; set; }
        [MaxLength(255)]
        public string msgEnglish { get; set; }

        [Required]
        public bool enabledFrench { get; set; }
        [MaxLength(255)]
        public string msgFrench { get; set; }

        [Required]
        public bool enabledGerman { get; set; }
        [MaxLength(255)]
        public string msgGerman { get; set; }

        [Required]
        public bool enabledJapanese { get; set; }
        [MaxLength(255)]
        public string msgJapanese { get; set; }

        // todo: maxlength
        // todo: 改行不可
        // todo: CBT後 テキスト装飾

        [MaxLength(255)]
        public string titleEnglish { get; set; }

        [MaxLength(255)]
        public string titleFrench { get; set; }

        [MaxLength(255)]
        public string titleGerman { get; set; }

        [MaxLength(255)]
        public string titleJapanese { get; set; }
    }
}
