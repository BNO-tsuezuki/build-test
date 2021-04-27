namespace LogFile.Models.ApiServer
{
    public class SayChat
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public int ChatType { get; set; }
        public string GroupId { get; set; }
        public string MatchId { get; set; }
        public int Side { get; set; }
        public string Text { get; set; }
    }
}
