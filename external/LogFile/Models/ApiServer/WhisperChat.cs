namespace LogFile.Models.ApiServer
{
    public class WhisperChat
    {
        public long PlayerId { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
        public long TargetPlayerId { get; set; }
    }
}
