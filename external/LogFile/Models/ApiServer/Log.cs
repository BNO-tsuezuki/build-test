namespace LogFile.Models.ApiServer
{
    public class Log
    {
        public string Time { get; set; }
        public string Tag { get; set; }
        public App App { get; set; }
    }

    public class App
    {
        // API Server
        public ChangeCustomItem ChangeCustomItem { get; set; }
        public ChangePackItem ChangePackItem { get; set; }
        public ChangePalletItem ChangePalletItem { get; set; }
        public ChangePlayerExp ChangePlayerExp { get; set; }
        public CreatePlayer CreatePlayer { get; set; }
        public CurrentSessionCount CurrentSessionCount { get; set; }
        public LeavePlayer LeavePlayer { get; set; }
        public Login Login { get; set; }
        public Logout Logout { get; set; }
        public PingResults PingResults { get; set; }
        public ResponseFriend ResponseFriend { get; set; }
        public RuptureFriend RuptureFriend { get; set; }
        public SayChat SayChat { get; set; }
        public SendFriend SendFriend { get; set; }
        public SetAppOption SetAppOption { get; set; }
        public SetOpenType SetOpenType { get; set; }
        public TutorialProgress TutorialProgress { get; set; }
        public WhisperChat WhisperChat { get; set; }

        // Matching Server
        public EntryPlayer EntryPlayer { get; set; }
        public ResponseInvitationParty ResponseInvitationParty { get; set; }
        public SendInvitationParty SendInvitationParty { get; set; }
        public UpdateParty UpdateParty { get; set; }
    }
}
