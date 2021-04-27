namespace LogFile.Models.ApiServer
{
    public class LeavePlayer
    {
        public long PlayerId { get; set; }
        public string EntryDate { get; set; }
        public string Date { get; set; }
        public int Type { get; set; }
        public float? Rating { get; set; }
        public string MatchId { get; set; }
    }
}
