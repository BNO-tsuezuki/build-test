namespace LogFile.Models.DedicatedServer
{
    public class DamageHistory
    {
        public string match_id { get; set; }
        public string round_start_datetime { get; set; }
        public long? round_count { get; set; }
        public int inflict_damage_team_cd { get; set; }
        public int inflict_damage_offense_defense_cd { get; set; }
        public string inflict_damage_player_id { get; set; }
        public string inflict_damage_unit_id { get; set; }
        public string inflict_damage_armed_id { get; set; }
        public int take_damage_team_cd { get; set; }
        public int take_damage_offense_defense_cd { get; set; }
        public string take_damage_player_id { get; set; }
        public string take_damage_unit_id { get; set; }
        public long inflict_damage_quantity { get; set; }
        public long inflict_damage_quantity_shield { get; set; }
    }
}
