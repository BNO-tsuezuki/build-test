using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace evotool.Models
{
    public class PlayerOptionSettings
    {
        public IList<PlayerOptionSetting> Options;

        public static class ValueType
        {
            public const string Switch = "switch";
            public const string SwitchMs = "switch-ms";
            public const string Range = "range";
            public const string RangeMs = "range-ms";
            public const string Key = "key";
        }
    }

    public class PlayerOptionSetting
    {
        public int Category { get; set; }
        public int Value { get; set; }
        public string JpText { get; set; }
        public string EnText { get; set; }
        public string ValueType { get; set; }
        public bool IsLocal { get; set; }
        public JObject Values { get; set; }
    }

    public class SwitchSetting
    {
        public IList<SwitchItem> Items { get; set; }
        public int DefaultIndex { get; set; }
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

        public class SwitchMsItem
        {
            public string MsId { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
            public int Default { get; set; }
            public IList<SwitchItem> Items { get; set; }
        }
    }

    public class RangeSetting
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Default { get; set; }
    }

    public class RangeMsSetting
    {
        public IList<RangeMsItem> Items { get; set; }

        public class RangeMsItem
        {
            public string MsId { get; set; }
            public string JpText { get; set; }
            public string EnText { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }
            public int Default { get; set; }
        }
    }

    public class KeySetting
    {
        public int Default { get; set; }
    }
}
