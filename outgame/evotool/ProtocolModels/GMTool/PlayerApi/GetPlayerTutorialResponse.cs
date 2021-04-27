namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerTutorialResponse
    {
        public Tutorial tutorial { get; set; }

        public class Tutorial
        {
            public bool isFirstTutorialEnd { get; set; }
            public int tutorialProgress { get; set; }
            public string displayNameJapanese { get; set; }
            public string displayNameEnglish { get; set; }
        }
    }
}
