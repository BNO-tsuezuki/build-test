using System;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerResponse
    {
        public class _Player
        {
            // TODO: コメントアウトされている項目は各機能実装後に対応する

            public long PlayerId { get; set; }
            public string PlayerName { get; set; }
            public string PlatformId { get; set; }
            public string Account { get; set; }
            public string CountryCode { get; set; }
            //public string Domain { get; set; }
            //public string Language { get; set; }
            public DateTime CreatedDate { get; set; } // todo: ★内容要確認

            public DateTime Discipline { get; set; } // todo: ★内容要確認
            //public int WarningCounts { get; set; }

            public DateTime? LastLoginDate { get; set; }
            public DateTime? LogoutDate { get; set; }

            //public long RoyalScore { get; set; }
            //public long TotalPaidEC { get; set; }
            //public long Last40DaysPaidEC { get; set; }
            //public long PaidEC { get; set; }
            //public long FreeEC { get; set; }

            //public string Rate { get; set; }
            //public string HighestRate { get; set; }
        }

        public _Player Player { get; set; }
    }
}
