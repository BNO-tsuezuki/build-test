using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerMobileSuitOptionResponse
    {
        public IList<Option> Options { get; set; }

        public class Option
        {
            public int OptionNo { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
            public string ValueType { get; set; }
            public bool IsLocal { get; set; }

            public int Value { get; set; }

            public SwitchSetting SwitchSetting { get; set; }
            public SwitchMsSetting SwitchMsSetting { get; set; }
            public RangeSetting RangeSetting { get; set; }
            public RangeMsSetting RangeMsSetting { get; set; }
        }

        public class SwitchSetting
        {
            public IList<SwitchItem> Items { get; set; }
        }

        public class SwitchItem
        {
            public int Index { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
        }

        public class SwitchMsSetting
        {
            public IList<SwitchMsItem> Items { get; set; }
            public bool IsCommonSettingAvailable { get; set; }
        }

        public class SwitchMsItem
        {
            public int Index { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
        }

        public class RangeSetting
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }

        public class RangeMsSetting
        {
            public int Min { get; set; }
            public int Max { get; set; }
            public bool IsCommonSettingAvailable { get; set; }
        }
    }
}
