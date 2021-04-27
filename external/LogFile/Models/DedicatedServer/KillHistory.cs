namespace LogFile.Models.DedicatedServer
{
    public class KillHistory
    {
        public string match_id { get; set; }
        public string kill_datetime { get; set; }
        public int kill_team_cd { get; set; }
        public int kill_offense_defense_cd { get; set; }
        public string kill_player_id { get; set; }
        public string kill_unit_id { get; set; }
        public string kill_armed_id { get; set; }
        public Vector kill_point { get; set; }
        public int killed_team_cd { get; set; }
        public int killed_offense_defense_cd { get; set; }
        public string killed_player_id { get; set; }
        public string killed_unit_id { get; set; }
        public Vector killed_point { get; set; }
        public int environment_kill_flag { get; set; }
    }

    public class Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
