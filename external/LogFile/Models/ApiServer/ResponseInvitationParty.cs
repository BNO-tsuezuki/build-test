namespace LogFile.Models.ApiServer
{
    public class ResponseInvitationParty
    {
        public long PlayerIdSrc { get; set; }
        public long PlayerIdDst { get; set; }
        public string Date { get; set; }
        public int Type { get; set; }
    }
}
