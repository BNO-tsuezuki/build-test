namespace LogFile.Models.ApiServer
{
    public static class LogType
    {
        // API Server
        public const string BanPlayer               = "App.BanPlayer";
        public const string ChangeCustomItem        = "App.ChangeCustomItem";
        public const string ChangePackItem          = "App.ChangePackItem";
        public const string ChangePalletItem        = "App.ChangePalletItem";
        public const string ChangePlayerExp         = "App.ChangePlayerExp";
        public const string ChangePlayerName        = "App.ChangePlayerName";
        public const string ChangePretendOffline    = "App.ChangePretendOffline";
        public const string CreatePlayer            = "App.CreatePlayer";
        public const string CurrentSessionCount     = "App.CurrentSessionCount";
        public const string DeletePlayer            = "App.DeletePlayer";
        public const string LeavePlayer             = "App.LeavePlayer";
        public const string Login                   = "App.Login";
        public const string Logout                  = "App.Logout";
        public const string PingResults             = "App.PingResults";
        public const string ResponseFriend          = "App.ResponseFriend";
        public const string RuptureFriend           = "App.RuptureFriend";
        public const string SayChat                 = "App.SayChat";
        public const string SendFriend              = "App.SendFriend";
        public const string SetAppOption            = "App.SetAppOption";
        public const string SetOpenType             = "App.SetOpenType";
        public const string TutorialProgress        = "App.TutorialProgress";
        public const string WhisperChat             = "App.WhisperChat";

        // Matching Server
        public const string EntryPlayer             = "App.EntryPlayer";
        public const string ResponseInvitationParty = "App.ResponseInvitationParty";
        public const string SendInvitationParty     = "App.SendInvitationParty";
        public const string UpdateParty             = "App.UpdateParty";
    }
}
