using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MasterDataProcess.Models.Translation
{
    public class TranslationDataJson
    {
        [JsonPropertyName("text_item")]
        public TranslationTextsCommon Item { get; set; }

        [JsonPropertyName("text_careerrecord")]
        public TranslationTextsCommon CareerRecord { get; set; }

        [JsonPropertyName("text_mobilesuit")]
        public TranslationTextsCommon MobileSuit { get; set; }

        [JsonPropertyName("text_weapon")]
        public TranslationTextsCommon Weapon { get; set; }

        [JsonPropertyName("text_tutorial")]
        public TranslationTextsCommon Tutorial { get; set; }

        [JsonPropertyName("text_level")]
        public TranslationTextsCommon Level { get; set; }

        [JsonPropertyName("text_option")]
        public JsonElement Option { get; set; }
    }

    public class TranslationTextsCommon
    {
        [JsonPropertyName("texts")]
        public IList<TranslationItemCommon> Texts { get; set; }
    }

    public class TranslationItemCommon
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("jpText")]
        public string JpText { get; set; }

        [JsonPropertyName("enText")]
        public string EnText { get; set; }
    }
}
