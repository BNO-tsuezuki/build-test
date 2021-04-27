using System.Collections.Generic;
using System.Linq;
using evotool.Models;
using Newtonsoft.Json;

namespace evotool.Services.GMTool
{
    public interface ITranslationService
    {
        IDictionary<string, TranslationItemCommon> GetCareerRecordDictionary();
        IDictionary<string, TranslationItemCommon> GetItemDictionary();
        IDictionary<string, TranslationItemCommon> GetMobileSuitDictionary();
        IDictionary<string, TranslationItemCommon> GetSeasonDictionary();
        IDictionary<string, TranslationItemCommon> GetTutorialDictionary();
        PlayerOptionSettings GetPlayerOptionSettings();
    }

    public class TranslationService : BaseService, ITranslationService
    {
        public TranslationService(IServicePack servicePack) : base(servicePack)
        { }

        public IDictionary<string, TranslationItemCommon> GetCareerRecordDictionary()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.CareerRecord.Texts.ToDictionary(x => x.Id);
        }

        public IDictionary<string, TranslationItemCommon> GetItemDictionary()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.Item.Texts.ToDictionary(x => x.Id);
        }

        public IDictionary<string, TranslationItemCommon> GetMobileSuitDictionary()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.MobileSuit.Texts.ToDictionary(x => x.Id);
        }

        public IDictionary<string, TranslationItemCommon> GetSeasonDictionary()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.Season.Texts.ToDictionary(x => x.Id);
        }

        public IDictionary<string, TranslationItemCommon> GetTutorialDictionary()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.Tutorial.Texts.ToDictionary(x => x.Id);
        }

        public PlayerOptionSettings GetPlayerOptionSettings()
        {
            var translationDataJson = JsonConvert.DeserializeObject<TranslationDataJson>(TranslationTable.RawData);

            return translationDataJson.Option.ToObject<PlayerOptionSettings>();
        }
    }
}
