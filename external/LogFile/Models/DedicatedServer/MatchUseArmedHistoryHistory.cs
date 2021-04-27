namespace LogFile.Models.DedicatedServer
{
    public class MatchUseArmedHistory
    {
        public string match_id { get; set; }
        public int team_cd { get; set; }
        public string round_start_datetime { get; set; }
        public long? round_count { get; set; }
        public int offense_defense_cd { get; set; }
        public string player_id { get; set; }
        public string unit_id { get; set; }
        public string armed_id { get; set; }
        public long use_armed_count { get; set; }
        public long bullet_fire_count { get; set; }
        public long bullet_hit_count { get; set; }
        public long bullet_headshot_count { get; set; }
        public long kill_count { get; set; }
        public long heal_quantity { get; set; }
        public long block_damage_quantity { get; set; }
        public long brake_shield_count { get; set; }
        public long using_armed_time_second { get; set; }
        public long brake_placement_object_count { get; set; }
        public long heal_assist_count { get; set; }
        public long enhance_assist_count { get; set; }
        public long weakness_assist_count { get; set; }
    }
}
