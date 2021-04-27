namespace LogFile.Models.DedicatedServer
{
    public class GetHaroHistory
    {
        public string match_id { get; set; }
        public string get_haro_datetime { get; set; }
        public string haro_placement_point { get; set; }
        public int team_cd { get; set; }
        public int offense_defense_cd { get; set; }
        public string player_id { get; set; }
        public string unit_id { get; set; }
    }
}
