namespace LogFile.Models.DedicatedServer
{
    public class MatchUseUnitHistory
    {
        public string match_id { get; set; }
        public int team_cd { get; set; }
        public string round_start_datetime { get; set; }
        public long? round_count { get; set; }
        public int offense_defense_cd { get; set; }
        public string player_id { get; set; }
        public string unit_id { get; set; }
        public long first_pick_flag { get; set; }
        public long sally_count { get; set; }
        public long sally_time_second { get; set; }
        public long inflict_damage_quantity { get; set; }
        public long take_damage_quantity { get; set; }
        public long kill_count { get; set; }
        public long death_count { get; set; }
        public long finalblow_count { get; set; }
        public long solo_kill_count { get; set; }
        public long continuity_kill_count { get; set; }
        public long environment_kill_count { get; set; }
        public long objective_kill_count_point { get; set; }
        public long objective_kill_count_domi { get; set; }
        public long objective_kill_count_dest { get; set; }
        public long objective_time_second_point { get; set; }
        public long objective_time_second_domi { get; set; }
        public long item_heal_quantity { get; set; }
        public long armed_heal_quantity { get; set; }
        public long normal_recovery_count { get; set; }
        public long armed_recovery_count { get; set; }
        public long half_evolution_time_second { get; set; }
        public long evolution_time_second { get; set; }
        public long base_control_time_second { get; set; }
        public long base_control_count { get; set; }
        public long base_gain_point { get; set; }
        public long placement_bomb_count { get; set; }
        public long disarm_bomb_time_second { get; set; }
        public long disarm_bomb_trial_count { get; set; }
        public long disarm_bomb_success_count { get; set; }
    }
}
