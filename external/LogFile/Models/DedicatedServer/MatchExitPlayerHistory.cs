namespace LogFile.Models.DedicatedServer
{
    public class MatchExitPlayerHistory
    {
        public string match_id { get; set; }
        public int team_cd { get; set; }
        public int match_format_cd { get; set; }
        public string player_id { get; set; }
        public string match_exit_datetime { get; set; }
        public int match_exit_reason_cd { get; set; }
    }
}
