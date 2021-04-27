namespace LogFile.Models.ApiServer
{
    public class CreatePlayer
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public string PlayerName { get; set; }
        public int AccountType { get; set; }
        public string CountryCode { get; set; }
    }
}
