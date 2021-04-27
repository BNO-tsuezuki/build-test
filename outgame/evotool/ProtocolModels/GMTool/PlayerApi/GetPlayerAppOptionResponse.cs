using System.Collections.Generic;

namespace evotool.ProtocolModels.GMTool.PlayerApi
{
    public class GetPlayerAppOptionResponse
    {
        public IList<Option> Options { get; set; }

        public class Option
        {
            public int Category { get; set; }
            public int OptionNo { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
            public string ValueType { get; set; }
            public bool IsLocal { get; set; }

            public int Value { get; set; }

            public SwitchSetting SwitchSetting { get; set; }
            public RangeSetting RangeSetting { get; set; }
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

        public class RangeSetting
        {
            public int Min { get; set; }
            public int Max { get; set; }
        }
    }
}
