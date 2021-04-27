namespace LogFile.Models.ApiServer
{
    public class ChangePlayerExp
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public int Exp { get; set; }
        public int TotalExp { get; set; }
        public int Level { get; set; }
    }
}
