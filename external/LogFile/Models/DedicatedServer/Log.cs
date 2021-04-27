namespace LogFile.Models.DedicatedServer
{
    public class Log
    {
        public string time { get; set; }
        public string Tag { get; set; }
        public Battle Battle { get; set; }
    }

    public class Battle
    {
        public DamageHistory damage_history { get; set; }
        public GetHaroHistory get_haro_history { get; set; }
        public KillHistory kill_history { get; set; }
        public MatchEntryPlayerHistory match_entry_player_history { get; set; }
        public MatchExecutionHistory match_execution_history { get; set; }
        public MatchExitPlayerHistory match_exit_player_history { get; set; }
        public MatchStartPlayerHistory match_start_player_history { get; set; }
        public MatchUseArmedHistory match_use_armed_history { get; set; }
        public MatchUseUnitHistory match_use_unit_history { get; set; }
        public PlayerRecordHistory player_record_history { get; set; }
        public UseCustomItemHistory use_custom_item_history { get; set; }
    }
}
