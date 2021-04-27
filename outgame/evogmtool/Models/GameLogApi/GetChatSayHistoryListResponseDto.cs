using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetChatSayHistoryListResponseDto : GameLogResponseBaseDto<ChatSayHistoryResponse>
    { }

    public class ChatSayHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public int ChatType { get; set; }
        public string GroupId { get; set; }
        public string MatchId { get; set; }
        public int? Side { get; set; }
        public string Text { get; set; }
    }
}
