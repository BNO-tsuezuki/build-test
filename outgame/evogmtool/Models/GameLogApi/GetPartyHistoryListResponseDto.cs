using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetPartyHistoryListResponseDto : GameLogResponseBaseDto<PartyHistoryResponse>
    { }

    public class PartyHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string GroupId { get; set; }
        public long PlayerId { get; set; }
        public int Type { get; set; }
        public long? PlayerId1 { get; set; }
        public long? PlayerId2 { get; set; }
        public long? PlayerId3 { get; set; }
        public long? PlayerId4 { get; set; }
        public long? PlayerId5 { get; set; }
    }
}
