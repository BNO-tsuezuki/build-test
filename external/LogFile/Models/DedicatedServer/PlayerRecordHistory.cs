namespace LogFile.Models.DedicatedServer
{
    public class PlayerRecordHistory
    {
        public string player_id { get; set; }
        public string unit_id { get; set; }
        public long player_level { get; set; }
        public long play_time_second { get; set; }
        public long match_count { get; set; }
        public long match_win_count { get; set; }
        public long match_draw_count { get; set; }
        public long match_lose_count { get; set; }
        public long half_evolution_time_second { get; set; }
        public long evolution_time_second { get; set; }
        public long finalblow_count { get; set; }
        public long solo_kill_count { get; set; }
        public long continuous_kill_count { get; set; }
        public long environment_kill_count { get; set; }
        public long death_count { get; set; }
        public long objective_kill_count_point { get; set; }
        public long objective_kill_count_domi { get; set; }
        public long objective_kill_count_dest { get; set; }
        public long objective_time_second_point { get; set; }
        public long objective_time_second_domi { get; set; }
        public long bullet_fire_count { get; set; }
        public long bullet_hit_count { get; set; }
        public long inflict_damage_quantity_all { get; set; }
        public long inflict_damage_quantity_shield { get; set; }
        public long inflict_damage_quantity_unit { get; set; }
        public long take_damage_quantity { get; set; }
        public long block_damage_quantity { get; set; }
        public long recovery_count { get; set; }
        public long setup_bomb_count { get; set; }
        public long release_bomb_count { get; set; }
        public string update_datetime { get; set; }
    }
}
