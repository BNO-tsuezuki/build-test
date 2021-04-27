namespace LogFile.Models.DedicatedServer
{
    public class MatchExecutionHistory
    {
        public string match_id { get; set; }
        public string match_start_datetime { get; set; }
        public string match_end_datetime { get; set; }
        public int match_format_cd { get; set; }
        public int rule_format_cd { get; set; }
        public string map_id { get; set; }
        public int match_end_reason_cd { get; set; }
        public int match_win_team_cd { get; set; }
        public int match_lose_team_cd { get; set; }
    }
}
