using System;

namespace evogmtool.Models.GameLogApi
{
    public class GetChatDirectHistoryListResponseDto : GameLogResponseBaseDto<ChatDirectHistoryResponse>
    { }

    public class ChatDirectHistoryResponse
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public long PlayerId { get; set; }
        public string Text { get; set; }
        public long TargetPlayerId { get; set; }
    }
}
