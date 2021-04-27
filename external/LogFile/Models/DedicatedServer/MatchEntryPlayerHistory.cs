namespace LogFile.Models.DedicatedServer
{
    public class MatchEntryPlayerHistory
    {
        public string match_id { get; set; }
        public int team_cd { get; set; }
        public int match_format_cd { get; set; }
        public string player_id { get; set; }
        public string match_entry_datetime { get; set; }
        public long party_no { get; set; }
        public long party_member_count { get; set; }
        public int match_entry_reason_cd { get; set; }
        public int player_authority { get; set; }
    }
}
