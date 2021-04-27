using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace evotool.Models
{
    public class TranslationDataJson
    {
        [JsonProperty("text_item")]
        public TranslationTextsCommon Item { get; set; }

        [JsonProperty("text_careerrecord")]
        public TranslationTextsCommon CareerRecord { get; set; }

        [JsonProperty("text_mobilesuit")]
        public TranslationTextsCommon MobileSuit { get; set; }

        [JsonProperty("text_weapon")]
        public TranslationTextsCommon Weapon { get; set; }

        [JsonProperty("text_season")]
        public TranslationTextsCommon Season { get; set; }

        [JsonProperty("text_tutorial")]
        public TranslationTextsCommon Tutorial { get; set; }

        [JsonProperty("text_option")]
        public JObject Option { get; set; }
    }

    public class TranslationTextsCommon
    {
        [JsonProperty("texts")]
        public IList<TranslationItemCommon> Texts { get; set; }
    }

    public class TranslationItemCommon
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("jpText")]
        public string JpText { get; set; }

        [JsonProperty("enText")]
        public string EnText { get; set; }
    }
}
